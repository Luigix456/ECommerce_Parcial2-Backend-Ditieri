using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Exceptions;

namespace ECommerce.Application.UseCases.Products.Commands;

public class CreateProductCommandUseCase
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;

    public CreateProductCommandUseCase(
        IProductRepository productRepository,
        ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<Product> ExecuteAsync(CreateProductCommand command, CancellationToken ct = default)
    {
        var category = await _categoryRepository.GetByIdAsync(command.CategoryId, ct);

        if (category is null)
            throw new NotFoundException("Category", command.CategoryId);

        var product = Product.Create(
            command.Name,
            command.Description,
            command.Price,
            command.Stock,
            command.CategoryId);

        await _productRepository.AddAsync(product, ct);

        return await _productRepository.GetByIdAsync(product.Id, ct) ?? product;
    }
}
