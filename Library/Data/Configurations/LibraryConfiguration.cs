using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Data.Configurations;

public class LibraryConfiguration : IEntityTypeConfiguration<Models.Library>
{
    public void Configure(EntityTypeBuilder<Models.Library> entity)
    {
        entity.HasKey(e => e.Id);

        entity.Property(x => x.Id)
              .ValueGeneratedOnAdd()
              .HasValueGenerator<UuidV7ValueGenerator>();

        entity.Property(e => e.Location)
              .IsRequired()
              .HasMaxLength(100);

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
    }
}