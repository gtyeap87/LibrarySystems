using Library.Api.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Library.Api.Data
{
    public static class Extensions
    {
        public static async Task ApplyMigrations(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await dbContext.Database.MigrateAsync();
        }
    }
}