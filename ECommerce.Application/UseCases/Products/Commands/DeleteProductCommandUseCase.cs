using ECommerce.Application.Interfaces;
using ECommerce.Domain.Exceptions;

namespace ECommerce.Application.UseCases.Products.Commands;

public class DeleteProductCommandUseCase
{
    private readonly IProductRepository _productRepository;

    public DeleteProductCommandUseCase(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task ExecuteAsync(DeleteProductCommand command, CancellationToken ct = default)
    {
        var product = await _productRepository.GetByIdAsync(command.Id, ct);

        if (product is null)
            throw new NotFoundException("Product", command.Id);

        await _productRepository.DeleteAsync(command.Id, ct);
    }
}
