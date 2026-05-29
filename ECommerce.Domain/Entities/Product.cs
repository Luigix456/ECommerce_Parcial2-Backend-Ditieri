using ECommerce.Domain.Exceptions;

namespace ECommerce.Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public int Stock { get; private set; }
    public Guid CategoryId { get; private set; }
    public Category? Category { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Product() { }

    private Product(string name, string description, decimal price, int stock, Guid categoryId)
    {
        Validate(name, price, stock, categoryId);

        Id = Guid.NewGuid();
        Name = name.Trim();
        Description = description?.Trim() ?? string.Empty;
        Price = price;
        Stock = stock;
        CategoryId = categoryId;
        CreatedAt = DateTime.UtcNow;
    }

    public static Product Create(
        string name,
        string description,
        decimal price,
        int stock,
        Guid categoryId
    )
    {
        return new Product(name, description, price, stock, categoryId);
    }

    public void ReserveStock(int quantity)
    {
        if (quantity <= 0)
            throw new DomainRuleException("La cantidad debe ser mayor a cero.");

        if (quantity > Stock)
            throw new InsufficientStockException(quantity, Stock);

        Stock -= quantity;
    }

    public void UpdatePrice(decimal newPrice)
    {
        if (newPrice <= 0)
            throw new DomainRuleException("El precio debe ser mayor a cero.");

        Price = newPrice;
    }

    private static void Validate(string name, decimal price, int stock, Guid categoryId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainRuleException("El nombre del producto es obligatorio.");

        if (price <= 0)
            throw new DomainRuleException("El precio debe ser mayor a cero.");

        if (stock < 0)
            throw new DomainRuleException("El stock no puede ser negativo.");

        if (categoryId == Guid.Empty)
            throw new DomainRuleException("La categoría es obligatoria.");
    }
}
