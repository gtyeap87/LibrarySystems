using Library.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Data.Configurations;

public class MemberConfiguration : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> entity)
    {
        entity.HasKey(e => e.Id);

        entity.Property(x => x.Id)
              .ValueGeneratedOnAdd()
              .HasValueGenerator<UuidV7ValueGenerator>();

        entity.Property(e => e.Name)
              .IsRequired()
              .HasMaxLength(100);

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
              .WithOne(lb => lb.Member)
              .HasForeignKey(lb => lb.MemberId)
              .OnDelete(DeleteBehavior.Restrict);
    }
}