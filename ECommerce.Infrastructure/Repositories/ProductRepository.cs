using ECommerce.Application.Common;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context
            .Products.Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id, ct);
    }

    public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context
            .Products.AsNoTracking()
            .Include(p => p.Category)
            .OrderBy(p => p.Name)
            .ToListAsync(ct);
    }

    public async Task<PagedResult<Product>> GetPagedAsync(
        int page,
        int pageSize,
        CancellationToken ct = default
    )
    {
        var query = _context.Products.AsNoTracking().Include(p => p.Category).OrderBy(p => p.Name);

        var totalCount = await query.CountAsync(ct);

        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);

        return new PagedResult<Product>(items, totalCount, page, pageSize);
    }

    public async Task<IEnumerable<Product>> SearchByNameAsync(
        string term,
        CancellationToken ct = default
    )
    {
        return await _context
            .Products.AsNoTracking()
            .Include(p => p.Category)
            .Where(p => p.Name.Contains(term))
            .OrderBy(p => p.Name)
            .ToListAsync(ct);
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.Products.AnyAsync(p => p.Id == id, ct);
    }

    public async Task AddAsync(Product product, CancellationToken ct = default)
    {
        await _context.Products.AddAsync(product, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Product product, CancellationToken ct = default)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var product = await _context.Products.FindAsync(new object[] { id }, ct);

        if (product is null)
            return;

        _context.Products.Remove(product);
        await _context.SaveChangesAsync(ct);
    }
}
