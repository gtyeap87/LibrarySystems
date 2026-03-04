using Library.Data.Contexts;
using Library.Data.Identities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;

namespace Library.Authorization
{
    public static class Extensions
    {
        public static async Task SeedRolesAndPermissions(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var adminRole = await roleManager.FindByNameAsync(Roles.Admin);
            if (adminRole is null)
            {
                adminRole = new IdentityRole(Roles.Admin);
                await roleManager.CreateAsync(adminRole);

                #region Users

                await roleManager.AddClaimAsync(adminRole,
                    new Claim(CustomClaimTypes.Permissions, Permissions.CreateUsers));

                await roleManager.AddClaimAsync(adminRole,
                    new Claim(CustomClaimTypes.Permissions, Permissions.ReadUsers));

                await roleManager.AddClaimAsync(adminRole,
                    new Claim(CustomClaimTypes.Permissions, Permissions.UpdateUsers));

                await roleManager.AddClaimAsync(adminRole,
                    new Claim(CustomClaimTypes.Permissions, Permissions.DeleteUsers));

                await roleManager.AddClaimAsync(adminRole,
                    new Claim(CustomClaimTypes.Permissions, Permissions.ChangePasswordUsers));

                #endregion Users

                #region Roles

                await roleManager.AddClaimAsync(adminRole,
                    new Claim(CustomClaimTypes.Permissions, Permissions.CreateRoles));

                await roleManager.AddClaimAsync(adminRole,
                    new Claim(CustomClaimTypes.Permissions, Permissions.ReadRoles));

                await roleManager.AddClaimAsync(adminRole,
                    new Claim(CustomClaimTypes.Permissions, Permissions.UpdateRoles));

                await roleManager.AddClaimAsync(adminRole,
                    new Claim(CustomClaimTypes.Permissions, Permissions.DeleteRoles));

                #endregion Roles
            }

            var librarianRole = await roleManager.FindByNameAsync(Roles.Librarian);
            if (librarianRole is null)
            {
                librarianRole = new IdentityRole(Roles.Librarian);
                await roleManager.CreateAsync(librarianRole);

                #region Books

                await roleManager.AddClaimAsync(librarianRole,
                    new Claim(CustomClaimTypes.Permissions, Permissions.ReadBooks));

                await roleManager.AddClaimAsync(librarianRole,
                    new Claim(CustomClaimTypes.Permissions, Permissions.CreateBooks));

                await roleManager.AddClaimAsync(librarianRole,
                    new Claim(CustomClaimTypes.Permissions, Permissions.UpdateBooks));

                await roleManager.AddClaimAsync(librarianRole,
                    new Claim(CustomClaimTypes.Permissions, Permissions.DeleteBooks));

                #endregion Books

                #region Members

                await roleManager.AddClaimAsync(librarianRole,
                    new Claim(CustomClaimTypes.Permissions, Permissions.ReadMembers));

                await roleManager.AddClaimAsync(librarianRole,
                    new Claim(CustomClaimTypes.Permissions, Permissions.CreateMembers));

                await roleManager.AddClaimAsync(librarianRole,
                    new Claim(CustomClaimTypes.Permissions, Permissions.UpdateMembers));

                await roleManager.AddClaimAsync(librarianRole,
                    new Claim(CustomClaimTypes.Permissions, Permissions.DeleteMembers));

                #endregion Members

                #region LoanBooks

                await roleManager.AddClaimAsync(librarianRole,
                    new Claim(CustomClaimTypes.Permissions, Permissions.ReadLoanBooks));

                await roleManager.AddClaimAsync(librarianRole,
                    new Claim(CustomClaimTypes.Permissions, Permissions.CreateLoanBooks));

                await roleManager.AddClaimAsync(librarianRole,
                    new Claim(CustomClaimTypes.Permissions, Permissions.UpdateLoanBooks));

                await roleManager.AddClaimAsync(librarianRole,
                    new Claim(CustomClaimTypes.Permissions, Permissions.DeleteLoanBooks));

                #endregion LoanBooks
            }

            var memberRole = await roleManager.FindByNameAsync(Roles.Member);
            if (memberRole is null)
            {
                memberRole = new IdentityRole(Roles.Member);
                await roleManager.CreateAsync(memberRole);

                await roleManager.AddClaimAsync(memberRole,
                 new Claim(CustomClaimTypes.Permissions, Permissions.ReadBooks));
                await roleManager.AddClaimAsync(memberRole,
                    new Claim(CustomClaimTypes.Permissions, Permissions.ReadLoanBooks));
            }
        }

        public static async Task SeedAdminUser(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

            //get if there is an admin user
            var adminUsers = await userManager.GetUsersInRoleAsync(Roles.Admin);
            if (adminUsers.Count == 0)
            {
                //create admin user

                #region Validation

                var user = new ApplicationUser()
                {
                    FirstName = "Super",
                    LastName = "Admin",
                    UserName = "komododragon87@gmail.com",
                    Email = "komododragon87@gmail.com",
                    Initials = "Mr",
                    EnableNotifications = false,
                    TwoFactorEnabled = false
                };

                var adminPassword = configuration["Password:AdminPassword"]!;
                var passwordValidator = new PasswordValidator<ApplicationUser>();
                var passwordValidationResult = await passwordValidator.ValidateAsync(userManager, user, adminPassword);

                if (!passwordValidationResult.Succeeded)
                {
                    var errors = string.Join(", ", passwordValidationResult.Errors.Select(e => e.Description));
                    throw new ArgumentException($"Password validation failed: {errors}");
                }

                #endregion Validation

                #region Execute

                using var transaction = await context.Database.BeginTransactionAsync();

                var identityResult = await userManager.CreateAsync(user, adminPassword);

                if (!identityResult.Succeeded)
                {
                    var errors = string.Join(", ", identityResult.Errors.Select(e => e.Description));
                    throw new InvalidOperationException($"User creation failed: {errors}");
                }

                var roleResult = await userManager.AddToRoleAsync(user, Roles.Admin);
                if (!roleResult.Succeeded)
                {
                    var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                    throw new InvalidOperationException($"role creation failed: {errors}");
                }

                await transaction.CommitAsync();

                #endregion Execute
            }
        }
    }
}