using Library.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Data.Configurations;

public class LoanBookConfiguration : IEntityTypeConfiguration<LoanBook>
{
    public void Configure(EntityTypeBuilder<LoanBook> entity)
    {
        entity.HasKey(e => e.Id);

        entity.Property(x => x.Id)
              .ValueGeneratedOnAdd()
              .HasValueGenerator<UuidV7ValueGenerator>();

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
    }
}