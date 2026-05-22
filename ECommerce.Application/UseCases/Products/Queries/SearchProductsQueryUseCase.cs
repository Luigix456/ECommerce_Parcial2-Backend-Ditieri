using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.UseCases.Products.Queries;

public class SearchProductsQueryUseCase
{
    private readonly IProductRepository _productRepository;

    public SearchProductsQueryUseCase(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<Product>> ExecuteAsync(SearchProductsQuery query, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(query.Term))
            return await _productRepository.GetAllAsync(ct);

        return await _productRepository.SearchByNameAsync(query.Term.Trim(), ct);
    }
}
