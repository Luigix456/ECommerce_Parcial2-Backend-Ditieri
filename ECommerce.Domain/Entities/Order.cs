using ECommerce.Domain.Exceptions;

namespace ECommerce.Domain.Entities;

public class Order : BaseEntity
{
    public Guid UserId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public OrderStatus Status { get; private set; }
    public decimal Total { get; private set; }

    private readonly List<OrderItem> _items = new();
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    private Order() { }

    private Order(Guid userId)
    {
        if (userId == Guid.Empty)
            throw new DomainRuleException("El usuario es obligatorio para crear la orden.");

        Id = Guid.NewGuid();
        UserId = userId;
        CreatedAt = DateTime.UtcNow;
        Status = OrderStatus.Pending;
        Total = 0;
    }

    public static Order Create(Guid userId)
    {
        return new Order(userId);
    }

    public void AddItem(Product product, int quantity)
    {
        if (product is null)
            throw new DomainRuleException("El producto es obligatorio.");

        if (quantity <= 0)
            throw new DomainRuleException("La cantidad debe ser mayor a cero.");

        product.ReserveStock(quantity);

        var item = OrderItem.Create(Id, product.Id, product.Price, quantity);
        _items.Add(item);
        Total += item.Subtotal;
    }

    public void Confirm()
    {
        if (!_items.Any())
            throw new DomainRuleException("No se puede confirmar una orden sin items.");

        Status = OrderStatus.Confirmed;
    }

    public void Cancel()
    {
        if (Status == OrderStatus.Delivered)
            throw new DomainRuleException("No se puede cancelar una orden entregada.");

        Status = OrderStatus.Cancelled;
    }
}
