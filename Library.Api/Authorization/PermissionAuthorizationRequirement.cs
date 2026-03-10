using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Library.Api.Authorization
{
    ///// <summary>
    ///// Custom authorization requirement that validates user permissions from claims.
    ///// Works with minimal APIs.
    ///// </summary>
    //public class PermissionAuthorizationRequirement(
    //    params string[] allowedPermissions
    //    ) : AuthorizationHandler<PermissionAuthorizationRequirement>, IAuthorizationRequirement
    //{
    //    public string[] AllowedPermissions { get; } = allowedPermissions;

    //    protected override Task HandleRequirementAsync(
    //        AuthorizationHandlerContext context,
    //        PermissionAuthorizationRequirement requirement
    //        )
    //    {
    //        foreach (var permission in requirement.AllowedPermissions)
    //        {
    //            bool found = context.User.FindFirst(c =>
    //            c.Type == CustomClaimTypes.Permissions && c.Value == permission) is not null;
    //            if (found)
    //            {
    //                context.Succeed(requirement);
    //                break;
    //            }
    //        }
    //        return Task.CompletedTask;
    //    }
    //}

    //public static class PermissionExtensions
    //{
    //    public static void RequiredPermissions(
    //        this AuthorizationPolicyBuilder builder,
    //        params string[] allowedPermissions
    //        )
    //    {
    //        builder.AddRequirements(new PermissionAuthorizationRequirement(allowedPermissions));
    //    }
    //}

    /// <summary>
    /// Requirement that holds allowed permission names.
    /// </summary>
    public class PermissionAuthorizationRequirement(params string[] allowedPermissions) : IAuthorizationRequirement
    {
        public string[] AllowedPermissions { get; } = allowedPermissions ?? [];
    }

    /// <summary>
    /// Handler that checks the user's claims for any of the required permissions.
    /// Works with controller policies created by the policy provider or registered policies.
    /// </summary>
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionAuthorizationRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionAuthorizationRequirement requirement)
        {
            if (context?.User == null || requirement == null || requirement.AllowedPermissions.Length == 0)
            {
                return Task.CompletedTask;
            }

            foreach (var permission in requirement.AllowedPermissions)
            {
                var found = context.User.FindFirst(c =>
                    c.Type == CustomClaimTypes.Permissions && c.Value == permission) is not null;

                if (found)
                {
                    context.Succeed(requirement);
                    break;
                }
            }

            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// Helper extension so you can register a policy in code: options.AddPolicy("...", p => p.AddRequirements(...));
    /// </summary>
    public static class PermissionExtensions
    {
        public static void RequiredPermissions(
            this AuthorizationPolicyBuilder builder,
            params string[] allowedPermissions)
        {
            builder.AddRequirements(new PermissionAuthorizationRequirement(allowedPermissions));
        }
    }

    /// <summary>
    /// Dynamic policy provider that recognizes policies in the format:
    /// "Permission:PermissionA,PermissionB"
    /// It builds an <see cref="AuthorizationPolicy"/> with a
    /// <see cref="PermissionAuthorizationRequirement"/>.
    /// Register this provider with DI to enable [Authorize(Policy = "Permission:...")].
    /// </summary>
    public class PermissionPolicyProvider(IOptions<AuthorizationOptions> options) : DefaultAuthorizationPolicyProvider(options)
    {
        private const string PolicyPrefix = "Permission:";

        public override Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            if (policyName?.StartsWith(PolicyPrefix, StringComparison.OrdinalIgnoreCase) == true)
            {
                var perms = policyName[PolicyPrefix.Length..]
                    .Split([','], StringSplitOptions.RemoveEmptyEntries)
                    .Select(p => p.Trim())
                    .Where(p => !string.IsNullOrEmpty(p))
                    .ToArray();

                var policy = new AuthorizationPolicyBuilder()
                    .AddRequirements(new PermissionAuthorizationRequirement(perms))
                    .Build();

                return Task.FromResult<AuthorizationPolicy?>(policy);
            }

            return base.GetPolicyAsync(policyName!);
        }
    }

    /// <summary>
    /// Attribute to apply permission-based policy on controllers/actions:
    /// [RequirePermission(Permissions.CreateUsers)]
    /// Produces a policy string "Permission:CreateUsers" that the provider recognizes.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class RequirePermissionAttribute : AuthorizeAttribute
    {
        private const string PolicyPrefix = "Permission:";

        public RequirePermissionAttribute(params string[] allowedPermissions)
        {
            Policy = PolicyPrefix + string.Join(",", allowedPermissions ?? []);
        }
    }
}