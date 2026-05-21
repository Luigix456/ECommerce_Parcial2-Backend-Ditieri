using ECommerce.Domain.Exceptions;

namespace ECommerce.Domain.Entities;

public class Order : BaseEntity
{
    public Guid UserId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public OrderStatus Status { get; private set; }
    public decimal Total { get; private set; }
    public List<OrderItem> Items { get; private set; } = new();

    private Order() { }

    public Order(Guid userId)
    {
        if (userId == Guid.Empty)
            throw new DomainRuleException("El usuario es obligatorio.");

        UserId = userId;
        CreatedAt = DateTime.UtcNow;
        Status = OrderStatus.Pending;
        Total = 0;
    }

    public void AddItem(Product product, int quantity)
    {
        if (product is null)
            throw new DomainRuleException("El producto es obligatorio.");
        if (!product.IsActive)
            throw new DomainRuleException("El producto no está activo.");

        product.Reserve(quantity);
        var item = new OrderItem(Id, product.Id, product.Price, quantity);
        Items.Add(item);
        Total += item.Subtotal;
    }

    public void Confirm()
    {
        if (Status != OrderStatus.Pending)
            throw new DomainRuleException("Solo se pueden confirmar órdenes pendientes.");
        Status = OrderStatus.Confirmed;
    }

    public void Cancel()
    {
        if (Status == OrderStatus.Delivered)
            throw new DomainRuleException("No se puede cancelar una orden entregada.");
        Status = OrderStatus.Cancelled;
    }
}
