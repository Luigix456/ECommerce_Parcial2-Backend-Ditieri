using ECommerce.Domain.Exceptions;

namespace ECommerce.Domain.Entities;

public class Category : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public List<Product> Products { get; private set; } = new();

    private Category() { }

    public Category(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainRuleException("El nombre de la categoría es obligatorio.");

        Name = name.Trim();
    }

    public void Rename(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainRuleException("El nombre de la categoría es obligatorio.");

        Name = name.Trim();
    }
}
