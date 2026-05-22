using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Exceptions;

namespace ECommerce.Application.UseCases.Products.Queries;

public class GetProductByIdQueryUseCase
{
    private readonly IProductRepository _productRepository;

    public GetProductByIdQueryUseCase(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Product> ExecuteAsync(GetProductByIdQuery query, CancellationToken ct = default)
    {
        var product = await _productRepository.GetByIdAsync(query.Id, ct);

        if (product is null)
            throw new NotFoundException("Product", query.Id);

        return product;
    }
}
