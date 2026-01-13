using Library.Data;
using Library.Data.Identity;
using Library.Model.Request;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Library.Service
{
    public class UserService(
        UserManager<ApplicationUser> userManager,
        ApplicationDbContext dbContext,
        IConfiguration configuration
        ) : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ApplicationDbContext _dbContext = dbContext;
        private readonly IConfiguration _configuration = configuration;

        public async Task<Guid> RegisterAsync(RegisterUserRequest request)
        {
            #region Validation

            if (string.IsNullOrEmpty(request.Email) || request.Email.Length == 0)
            {
                throw new ArgumentException("Email is required.");
            }

            var user = new ApplicationUser()
            {
                FirstName = request.FirstName,
                LastName = request.SecondName,
                UserName = request.Email,
                Email = request.Email,
                Initials = request.Initials,
                EnableNotifications = request.EnableNotification,
                TwoFactorEnabled = request.TwoFactorAuthentication
            };

            var passwordValidator = new PasswordValidator<ApplicationUser>();
            var passwordValidationResult = await passwordValidator.ValidateAsync(_userManager, user, request.Password);

            if (!passwordValidationResult.Succeeded)
            {
                var errors = string.Join(", ", passwordValidationResult.Errors.Select(e => e.Description));
                throw new ArgumentException($"Password validation failed: {errors}");
            }

            #endregion Validation

            #region Execute

            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            var identityResult = await _userManager.CreateAsync(user, request.Password);

            if (!identityResult.Succeeded)
            {
                var errors = string.Join(", ", identityResult.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"User creation failed: {errors}");
            }

            var roleResult = await _userManager.AddToRoleAsync(user, request.Role);
            if (!roleResult.Succeeded)
            {
                var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"role creation failed: {errors}");
            }

            await transaction.CommitAsync();

            return Guid.Parse(user.Id);

            #endregion Execute
        }

        public async Task DeleteAsync(DeleteUserRequest request)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            //check if the user is admin
            var loginUser = await _userManager.FindByIdAsync(request.LoginId) ?? throw new ArgumentException("Login user not found.");
            var users = await _userManager.GetRolesAsync(loginUser);
            if (!users.Contains(Roles.Admin))
            {
                throw new UnauthorizedAccessException("Only admin users can delete users.");
            }

            var deleteUser = await _userManager.FindByIdAsync(request.DeleteId) ?? throw new ArgumentException("Delete user not found.");
            var deleteResult = await _userManager.DeleteAsync(deleteUser);
            if (!deleteResult.Succeeded)
            {
                var errors = string.Join(", ", deleteResult.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"user delete failed: {errors}");
            }

            await transaction.CommitAsync();
        }

        public async Task<string> LoginAsync(LoginUserRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null || !await _userManager.CheckPasswordAsync(user, request.Password))
            {
                throw new UnauthorizedAccessException();
            }

            var roles = await _userManager.GetRolesAsync(user);

            var secretkey = _configuration["Jwt:SecretKey"]!;
            var signingKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretkey));
            var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            List<Claim> claims =
            [
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim("firstName", user.FirstName ?? string.Empty),
                new Claim("lastName", user.LastName ?? string.Empty),
                ..roles.Select(role => new Claim(ClaimTypes.Role, role))
            ];

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                Expires = DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("Jwt:ExpiryInMinutes")),
                SigningCredentials = credentials
            };

            var tokenHandler = new JsonWebTokenHandler();
            string accessToken = tokenHandler.CreateToken(tokenDescriptor);

            return accessToken;
        }
    }
}