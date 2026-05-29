using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> b)
    {
        b.ToTable("Users");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).ValueGeneratedNever();
        b.Property(x => x.Email).IsRequired().HasMaxLength(250);
        b.Property(x => x.Name).IsRequired().HasMaxLength(200);
        b.Property(x => x.PasswordHash).IsRequired();
        b.Property(x => x.Role).IsRequired().HasMaxLength(20);
        b.Property(x => x.CreatedAt).IsRequired();
        b.HasIndex(x => x.Email).IsUnique();
    }
}
