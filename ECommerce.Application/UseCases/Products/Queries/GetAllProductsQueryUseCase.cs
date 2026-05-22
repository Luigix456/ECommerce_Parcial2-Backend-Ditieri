using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.UseCases.Products.Queries;

public class GetAllProductsQueryUseCase
{
    private readonly IProductRepository _productRepository;

    public GetAllProductsQueryUseCase(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<Product>> ExecuteAsync(GetAllProductsQuery query, CancellationToken ct = default)
    {
        return await _productRepository.GetAllAsync(ct);
    }
}
