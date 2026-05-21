using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Persistence.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public static readonly Guid ElectronicaId = new("a1b2c3d4-0000-0000-0000-000000000001");
    public static readonly Guid RopaId = new("a1b2c3d4-0000-0000-0000-000000000002");
    public static readonly Guid HogarId = new("a1b2c3d4-0000-0000-0000-000000000003");

    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedNever();
        builder.Property(c => c.Name).IsRequired().HasMaxLength(100);
        builder.HasIndex(c => c.Name).IsUnique();

        builder.HasData(
            new { Id = ElectronicaId, Name = "Electrónica" },
            new { Id = RopaId, Name = "Ropa" },
            new { Id = HogarId, Name = "Hogar" }
        );
    }
}
