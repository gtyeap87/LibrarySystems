using Microsoft.AspNetCore.Identity;

namespace Library.Data.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public bool EnableNotifications { get; set; }
        public string? Initials { get; set; } = string.Empty;
    }
}