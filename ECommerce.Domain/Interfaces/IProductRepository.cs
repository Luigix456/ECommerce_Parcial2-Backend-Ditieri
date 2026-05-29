using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Interfaces;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<Product>> GetPagedAsync(
        int page,
        int pageSize,
        CancellationToken ct = default
    );
    Task<int> CountAsync(CancellationToken ct = default);
    Task AddAsync(Product product, CancellationToken ct = default);
}
