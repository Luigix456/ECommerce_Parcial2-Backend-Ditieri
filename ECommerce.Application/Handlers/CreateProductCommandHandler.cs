using ECommerce.Application.Commands;
using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Application.Mappings;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Exceptions;
using MediatR;

namespace ECommerce.Application.Handlers;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductDto>
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;

    public CreateProductCommandHandler(
        IProductRepository productRepository,
        ICategoryRepository categoryRepository
    )
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken ct)
    {
        var category = await _categoryRepository.GetByIdAsync(request.CategoryId, ct);
        if (category is null)
            throw new NotFoundException("Category", request.CategoryId);
        var product = Product.Create(
            request.Name,
            request.Description,
            request.Price,
            request.Stock,
            request.CategoryId
        );
        await _productRepository.AddAsync(product, ct);
        var saved = await _productRepository.GetByIdAsync(product.Id, ct) ?? product;
        return ProductMapper.ToDto(saved);
    }
}
