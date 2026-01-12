using Library.Model;
using Microsoft.EntityFrameworkCore;

namespace Library.Data;

public class LibraryContext(DbContextOptions<LibraryContext> options) : DbContext(options)
{
    public DbSet<Book> Books { get; set; }
    public DbSet<BookStock> BookStocks { get; set; }
    public DbSet<Member> Members { get; set; }
    public DbSet<Model.Library> Libraries { get; set; }
    public DbSet<LoanBook> LoanBooks { get; set; }

    /// <summary>
    /// Configure entity relationships and constraints
    /// </summary>
    /// <param name="builder"></param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Genre)
                  .IsRequired()
                  .HasDefaultValue(Genre.Unknown);

            entity.Property(e => e.Name)
                  .IsRequired();

            entity.Property(e => e.RowVersion)
                  .IsRowVersion();

            // Relationship: Book → Library (many-to-one)
            entity.HasOne<Model.Library>()
                  .WithMany(l => l.Books)
                  .HasForeignKey(e => e.LibraryId)
                  .OnDelete(DeleteBehavior.Cascade);

            // Relationship: Book → BookStock (one-to-many)
            entity.HasMany(e => e.BookStocks)
                  .WithOne()
                  .HasForeignKey(bs => bs.BookId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<BookStock>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Quantity)
                  .IsRequired();
            entity.Property(e => e.RowVersion)
                  .IsRowVersion();
            // Relationship: BookStock → Book (many-to-one)
            entity.HasOne<Book>()
                  .WithMany(b => b.BookStocks)
                  .HasForeignKey(e => e.BookId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<Member>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name)
                  .IsRequired();
            entity.Property(e => e.JoinedDate)
                  .IsRequired();
            entity.Property(e => e.RowVersion)
                  .IsRowVersion();
            // Relationship: Member → Library (many-to-one)
            entity.HasOne<Model.Library>()
                  .WithMany(l => l.Members)
                  .HasForeignKey(e => e.LibraryId)
                  .OnDelete(DeleteBehavior.Cascade);
            // Relationship: Member → LoanBook (one-to-many)
            entity.HasMany(e => e.LoanedBooks)
                  .WithOne()
                  .HasForeignKey(lb => lb.MemberId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<Model.Library>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Location)
                  .IsRequired();
            // Relationship: Library → Members (one-to-many)
            entity.HasMany(e => e.Members)
                  .WithOne()
                  .HasForeignKey(m => m.LibraryId)
                  .OnDelete(DeleteBehavior.Cascade);
            // Relationship: Library → Books (one-to-many)
            entity.HasMany(e => e.Books)
                  .WithOne()
                  .HasForeignKey(b => b.LibraryId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<LoanBook>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.LoanedDate)
                  .IsRequired();

            entity.Property(e => e.ReturnedDate);

            entity.Property(e => e.RowVersion)
                  .IsRowVersion();

            // Relationship: LoanBook → Member (many-to-one)
            entity.HasOne(e => e.Member)
                  .WithMany(m => m.LoanedBooks)
                  .HasForeignKey(e => e.MemberId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.Navigation(e => e.Member)
                  .AutoInclude();

            // Relationship: LoanBook → Book (many-to-one)
            entity.HasOne(e => e.Book)
                  .WithMany()
                  .HasForeignKey(e => e.BookId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.Navigation(e => e.Book)
                  .AutoInclude();
        });

        builder.HasDefaultSchema("lib");
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