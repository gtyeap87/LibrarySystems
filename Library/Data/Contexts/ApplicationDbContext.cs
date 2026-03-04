using Library.Data.Identities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Library.Data.Contexts
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Additional configurations can be added here
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(e => e.EnableNotifications).HasDefaultValue(true);
                entity.Property(e => e.Initials).HasMaxLength(10);
            });

            builder.HasDefaultSchema("identity");
        }
    }
}