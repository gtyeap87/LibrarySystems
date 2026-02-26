using Library.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Data.Configurations;

public class BookStockConfiguration : IEntityTypeConfiguration<BookStock>
{
    public void Configure(EntityTypeBuilder<BookStock> entity)
    {
        entity.HasKey(e => e.Id);

        entity.Property(x => x.Id)
              .ValueGeneratedOnAdd()
              .HasValueGenerator<UuidV7ValueGenerator>();

        entity.Property(e => e.Quantity)
              .IsRequired();

        entity.Property(e => e.RowVersion)
              .IsRowVersion();

        // Relationship: BookStock → Book (many-to-one)
        entity.HasOne<Book>()
              .WithMany(b => b.BookStocks)
              .HasForeignKey(e => e.BookId)
              .OnDelete(DeleteBehavior.Cascade);
    }
}