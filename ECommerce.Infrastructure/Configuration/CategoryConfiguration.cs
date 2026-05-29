using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Configuration;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedNever();

        builder.Property(c => c.Name).IsRequired().HasMaxLength(150);

        builder.HasIndex(c => c.Name).IsUnique();

        builder.HasData(
            new { Id = Guid.Parse("a1b2c3d4-0000-0000-0000-000000000001"), Name = "Electrónica" },
            new { Id = Guid.Parse("a1b2c3d4-0000-0000-0000-000000000002"), Name = "Hogar" },
            new { Id = Guid.Parse("a1b2c3d4-0000-0000-0000-000000000003"), Name = "Libros" }
        );
    }
}
