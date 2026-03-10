namespace Library.Api.Dto.Identities
{
    public record UserDto(
        Guid Id,
        string FirstName,
        string LastName,
        string Email,
        string Initials,
        bool EnableNotifications,
        bool TwoFactorEnabled,
        string UserName,
        bool LockoutEnabled,
        IList<string> Roles
        );
}