using ECommerce.Api.DTOs;
using ECommerce.Domain.Entities;

namespace ECommerce.Api.Mappers;

public static class OrderMapper
{
    public static OrderDto ToDto(Order order)
    {
        return new OrderDto
        {
            Id = order.Id,
            UserId = order.UserId,
            CreatedAt = order.CreatedAt,
            Status = order.Status.ToString(),
            Total = order.Total,
            Items = order.Items.Select(i => new OrderItemDto
            {
                Id = i.Id,
                ProductId = i.ProductId,
                UnitPrice = i.UnitPrice,
                Quantity = i.Quantity,
                Subtotal = i.Subtotal
            }).ToList()
        };
    }
}
