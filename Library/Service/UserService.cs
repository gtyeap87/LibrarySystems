using Library.Authorization;
using Library.Data;
using Library.Data.Identity;
using Library.Dto.Request;
using Library.Strategies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Library.Service
{
    public class UserService(
        UserManager<ApplicationUser> userManager,
        ApplicationDbContext dbContext,
        IConfiguration configuration,
        IStrategyHandler strategyHandler
        ) : IUserService
    {
        public async Task<Guid> RegisterUserAsync(RegisterUserRequest request)
        {
            #region Validation

            if (string.IsNullOrEmpty(request.Email) || request.Email.Length == 0)
            {
                throw new ArgumentException("Email is required.");
            }

            var user = new ApplicationUser()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.Email,
                Email = request.Email,
                Initials = request.Initials,
                EnableNotifications = request.EnableNotification,
                TwoFactorEnabled = request.TwoFactorAuthentication
            };

            await ValidatePassword(user, request.Password);

            #endregion Validation

            #region Execute

            using var transaction = await dbContext.Database.BeginTransactionAsync();

            var identityResult = await userManager.CreateAsync(user, request.Password);

            if (!identityResult.Succeeded)
            {
                var errors = string.Join(", ", identityResult.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"User creation failed: {errors}");
            }

            var roleResult = await userManager.AddToRoleAsync(user, request.Role);
            if (!roleResult.Succeeded)
            {
                var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"role creation failed: {errors}");
            }

            //apply strategies
            await strategyHandler.HandleAsync(request);

            await transaction.CommitAsync();

            return Guid.Parse(user.Id);

            #endregion Execute
        }

        public async Task<(ApplicationUser User, IList<string> Roles)> ReadUserAsync(Guid id)
        {
            var user = await userManager.FindByIdAsync(id.ToString()) ?? throw new InvalidOperationException("User not found.");
            var roles = await userManager.GetRolesAsync(user) ?? [];

            return (user, roles);
        }

        public async Task DeleteUserAsync(Guid deleteUserId)
        {
            using var transaction = await dbContext.Database.BeginTransactionAsync();

            var deleteUser = await userManager.FindByIdAsync(deleteUserId.ToString()) ?? throw new ArgumentException("Delete user not found.");
            var deleteResult = await userManager.DeleteAsync(deleteUser);
            if (!deleteResult.Succeeded)
            {
                var errors = string.Join(", ", deleteResult.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"user delete failed: {errors}");
            }

            await transaction.CommitAsync();
        }

        public async Task<string> LoginUserAsync(LoginUserRequest request)
        {
            var user = await userManager.FindByEmailAsync(request.Email);

            if (user is null || !await userManager.CheckPasswordAsync(user, request.Password))
            {
                throw new UnauthorizedAccessException();
            }

            var roles = await userManager.GetRolesAsync(user);

            var secretkey = configuration["Jwt:SecretKey"]!;
            var signingKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretkey));
            var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var permissions = await (
                from role in dbContext.Roles
                join claim in dbContext.RoleClaims on role.Id equals claim.RoleId
                where roles.Contains(role.Name!) && claim.ClaimType == CustomClaimTypes.Permissions
                select claim.ClaimValue
                )
                .Distinct()
                .ToArrayAsync();

            List<Claim> claims =
            [
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim("firstName", user.FirstName ?? string.Empty),
                new Claim("lastName", user.LastName ?? string.Empty),
                ..roles.Select(role => new Claim(ClaimTypes.Role, role)),
                ..permissions.Select(permission => new Claim(CustomClaimTypes.Permissions, permission))
            ];

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = configuration["Jwt:Issuer"],
                Audience = configuration["Jwt:Audience"],
                Expires = DateTime.UtcNow.AddMinutes(configuration.GetValue<int>("Jwt:ExpiryInMinutes")),
                SigningCredentials = credentials
            };

            var tokenHandler = new JsonWebTokenHandler();
            string accessToken = tokenHandler.CreateToken(tokenDescriptor);

            return accessToken;
        }

        public async Task UpdateUserAsync(Guid id, UpdateUserRequest request)
        {
            var user = await userManager.FindByIdAsync(id.ToString()) ?? throw new InvalidOperationException("User not found.");

            user.FirstName = request.FirstName switch
            {
                not null => request.FirstName,
                _ => user.FirstName
            };

            user.LastName = request.SecondName switch
            {
                not null => request.SecondName,
                _ => user.LastName
            };

            user.Initials = request.Initials switch
            {
                not null => request.Initials,
                _ => user.Initials
            };

            user.EnableNotifications = request.EnableNotification switch
            {
                not null => request.EnableNotification.Value,
                _ => user.EnableNotifications
            };

            user.TwoFactorEnabled = request.TwoFactorAuthentication switch
            {
                not null => request.TwoFactorAuthentication.Value,
                _ => user.TwoFactorEnabled
            };

            user.PhoneNumber = request.PhoneNumber switch
            {
                not null => request.PhoneNumber,
                _ => user.PhoneNumber
            };

            user.LockoutEnabled = request.LockoutEnabled switch
            {
                not null => request.LockoutEnabled.Value,
                _ => user.LockoutEnabled
            };

            if (request.LockoutEnd is not null)
                user.LockoutEnd = DateTime.UtcNow.AddYears(1);

            using var transaction = await dbContext.Database.BeginTransactionAsync();

            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"User update failed: {errors}");
            }

            await transaction.CommitAsync();
        }

        public async Task ChangePasswordAsync(ChangePasswordRequest request)
        {
            var user = await userManager.FindByEmailAsync(request.Email) ?? throw new UnauthorizedAccessException();

            if (!await userManager.CheckPasswordAsync(user, request.Password))
            {
                throw new InvalidOperationException("Wrong old password");
            }

            await ValidatePassword(user, request.NewPassword);

            var result = await userManager.ChangePasswordAsync(user, request.Password, request.NewPassword);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"User change password failed: {errors}");
            }
        }

        private async Task ValidatePassword(ApplicationUser user, string newPasword)
        {
            var passwordValidator = new PasswordValidator<ApplicationUser>();
            var passwordValidationResult = await passwordValidator.ValidateAsync(userManager, user, newPasword);

            if (!passwordValidationResult.Succeeded)
            {
                var errors = string.Join(", ", passwordValidationResult.Errors.Select(e => e.Description));
                throw new ArgumentException($"Password validation failed: {errors}");
            }
        }
    }
}