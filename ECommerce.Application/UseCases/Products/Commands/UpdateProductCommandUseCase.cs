using ECommerce.Application.Interfaces;
using ECommerce.Domain.Exceptions;

namespace ECommerce.Application.UseCases.Products.Commands;

public class UpdateProductCommandUseCase
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;

    public UpdateProductCommandUseCase(
        IProductRepository productRepository,
        ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task ExecuteAsync(UpdateProductCommand command, CancellationToken ct = default)
    {
        var product = await _productRepository.GetByIdAsync(command.Id, ct);

        if (product is null)
            throw new NotFoundException("Product", command.Id);

        var category = await _categoryRepository.GetByIdAsync(command.CategoryId, ct);

        if (category is null)
            throw new NotFoundException("Category", command.CategoryId);

        product.Update(
            command.Name,
            command.Description,
            command.Price,
            command.Stock,
            command.CategoryId);

        await _productRepository.UpdateAsync(product, ct);
    }
}
