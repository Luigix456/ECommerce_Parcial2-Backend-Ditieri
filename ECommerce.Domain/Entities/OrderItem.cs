using ECommerce.Domain.Exceptions;

namespace ECommerce.Domain.Entities;

public class OrderItem : BaseEntity
{
    public Guid OrderId { get; private set; }
    public Guid ProductId { get; private set; }
    public decimal UnitPrice { get; private set; }
    public int Quantity { get; private set; }
    public decimal Subtotal => UnitPrice * Quantity;

    private OrderItem() { }

    private OrderItem(Guid orderId, Guid productId, decimal unitPrice, int quantity)
    {
        if (orderId == Guid.Empty)
            throw new DomainRuleException("La orden es obligatoria.");

        if (productId == Guid.Empty)
            throw new DomainRuleException("El producto es obligatorio.");

        if (unitPrice <= 0)
            throw new DomainRuleException("El precio unitario debe ser mayor a cero.");

        if (quantity <= 0)
            throw new DomainRuleException("La cantidad debe ser mayor a cero.");

        Id = Guid.NewGuid();
        OrderId = orderId;
        ProductId = productId;
        UnitPrice = unitPrice;
        Quantity = quantity;
    }

    public static OrderItem Create(Guid orderId, Guid productId, decimal unitPrice, int quantity)
    {
        return new OrderItem(orderId, productId, unitPrice, quantity);
    }
}
