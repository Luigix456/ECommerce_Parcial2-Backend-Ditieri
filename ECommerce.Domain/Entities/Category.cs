using ECommerce.Domain.Exceptions;

namespace ECommerce.Domain.Entities;

public class Category : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public ICollection<Product> Products { get; private set; } = new List<Product>();

    private Category() { }

    private Category(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainRuleException("El nombre de la categoría es obligatorio.");
        Name = name.Trim();
    }

    public static Category Create(string name) => new(name);
}
