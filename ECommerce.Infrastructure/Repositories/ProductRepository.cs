using ECommerce.Application.Common;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _ctx;
    public ProductRepository(ApplicationDbContext ctx) => _ctx = ctx;

    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _ctx.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id, ct);

    public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken ct = default)
        => await _ctx.Products
            .AsNoTracking()
            .Include(p => p.Category)
            .Where(p => p.IsActive)
            .OrderBy(p => p.Name)
            .ToListAsync(ct);

    public async Task<IEnumerable<Product>> SearchByNameAsync(string term, CancellationToken ct = default)
        => await _ctx.Products
            .AsNoTracking()
            .Include(p => p.Category)
            .Where(p => p.IsActive && p.Name.Contains(term))
            .OrderBy(p => p.Name)
            .ToListAsync(ct);

    public async Task<PagedResult<Product>> GetPagedAsync(int page, int pageSize, CancellationToken ct = default)
    {
        if (page <= 0) page = 1;
        if (pageSize <= 0) pageSize = 10;

        var query = _ctx.Products
            .AsNoTracking()
            .Include(p => p.Category)
            .Where(p => p.IsActive)
            .OrderBy(p => p.Name);

        var totalCount = await query.CountAsync(ct);
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return new PagedResult<Product>(items, totalCount, page, pageSize);
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken ct = default)
        => await _ctx.Products.AnyAsync(p => p.Id == id, ct);

    public async Task AddAsync(Product product, CancellationToken ct = default)
    {
        await _ctx.Products.AddAsync(product, ct);
        await _ctx.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Product product, CancellationToken ct = default)
    {
        _ctx.Products.Update(product);
        await _ctx.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var product = await _ctx.Products.FindAsync(new object[] { id }, ct);
        if (product is null) return;

        product.Deactivate();
        await _ctx.SaveChangesAsync(ct);
    }
}
