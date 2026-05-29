using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Configuration;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItems");

        builder.HasKey(i => i.Id);
        builder.Property(i => i.Id).ValueGeneratedNever();

        builder.Property(i => i.OrderId).IsRequired();
        builder.Property(i => i.ProductId).IsRequired();

        builder.Property(i => i.UnitPrice).HasColumnType("decimal(18,2)").IsRequired();

        builder.Property(i => i.Quantity).IsRequired();

        builder.Ignore(i => i.Subtotal);

        builder.HasIndex(i => i.OrderId);
        builder.HasIndex(i => i.ProductId);
    }
}
