using Library.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Data.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> entity)
    {
        entity.HasKey(e => e.Id);

        entity.Property(x => x.Id)
              .ValueGeneratedOnAdd()
              .HasValueGenerator<UuidV7ValueGenerator>();

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
    }
}