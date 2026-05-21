using ECommerce.Api.DTOs;
using ECommerce.Api.Mappers;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;

    public ProductsController(IProductRepository productRepository, ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll(CancellationToken ct)
    {
        var products = await _productRepository.GetAllAsync(ct);
        return Ok(products.Select(ProductMapper.ToDto));
    }

    [HttpGet("paged")]
    public async Task<ActionResult<PagedResponse<ProductDto>>> GetPaged(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken ct = default)
    {
        var paged = await _productRepository.GetPagedAsync(page, pageSize, ct);
        return Ok(ProductMapper.ToPagedResponse(paged));
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<ProductDto>>> Search(
        [FromQuery] string term,
        CancellationToken ct)
    {
        var products = await _productRepository.SearchByNameAsync(term, ct);
        return Ok(products.Select(ProductMapper.ToDto));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProductDto>> GetById(Guid id, CancellationToken ct)
    {
        var product = await _productRepository.GetByIdAsync(id, ct);
        if (product is null)
            throw new NotFoundException("Product", id);

        return Ok(ProductMapper.ToDto(product));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<ProductDto>> Create([FromBody] CreateProductRequest request, CancellationToken ct)
    {
        var category = await _categoryRepository.GetByIdAsync(request.CategoryId, ct);
        if (category is null)
            throw new NotFoundException("Category", request.CategoryId);

        var product = Product.Create(
            request.Name,
            request.Description,
            request.Price,
            request.Stock,
            request.CategoryId);

        await _productRepository.AddAsync(product, ct);
        product = await _productRepository.GetByIdAsync(product.Id, ct) ?? product;

        return CreatedAtAction(nameof(GetById), new { id = product.Id }, ProductMapper.ToDto(product));
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductRequest request, CancellationToken ct)
    {
        var product = await _productRepository.GetByIdAsync(id, ct);
        if (product is null)
            throw new NotFoundException("Product", id);

        var category = await _categoryRepository.GetByIdAsync(request.CategoryId, ct);
        if (category is null)
            throw new NotFoundException("Category", request.CategoryId);

        product.Update(request.Name, request.Description, request.Price, request.Stock, request.CategoryId);
        await _productRepository.UpdateAsync(product, ct);

        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var product = await _productRepository.GetByIdAsync(id, ct);
        if (product is null)
            throw new NotFoundException("Product", id);

        await _productRepository.DeleteAsync(id, ct);
        return NoContent();
    }
}
