using ECommerce.Application.Common;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.UseCases.Products.Queries;

public class GetPagedProductsQueryUseCase
{
    private readonly IProductRepository _productRepository;

    public GetPagedProductsQueryUseCase(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<PagedResult<Product>> ExecuteAsync(GetPagedProductsQuery query, CancellationToken ct = default)
    {
        var page = query.Page <= 0 ? 1 : query.Page;
        var pageSize = query.PageSize <= 0 ? 10 : query.PageSize;

        return await _productRepository.GetPagedAsync(page, pageSize, ct);
    }
}
