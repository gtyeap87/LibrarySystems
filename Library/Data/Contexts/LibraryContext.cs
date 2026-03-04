using Library.Data.Configurations;
using Library.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Data.Contexts;

public class LibraryContext(DbContextOptions<LibraryContext> options) : DbContext(options)
{
    public DbSet<Book> Books { get; set; }
    public DbSet<BookStock> BookStocks { get; set; }
    public DbSet<Member> Members { get; set; }
    public DbSet<Models.Library> Libraries { get; set; }
    public DbSet<LoanBook> LoanBooks { get; set; }

    /// <summary>
    /// Configure entity relationships and constraints
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //LibraryConfiguration.Configure(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LibraryContext).Assembly);
        modelBuilder.HasDefaultSchema("lib");
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<Root>();

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.UtcNow;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.ModifiedAt = DateTime.UtcNow;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}