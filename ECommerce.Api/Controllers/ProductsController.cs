using ECommerce.Api.DTOs;
using ECommerce.Api.Mappers;
using ECommerce.Application.UseCases.Products.Commands;
using ECommerce.Application.UseCases.Products.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly GetAllProductsQueryUseCase _getAllProducts;
    private readonly GetProductByIdQueryUseCase _getProductById;
    private readonly GetPagedProductsQueryUseCase _getPagedProducts;
    private readonly SearchProductsQueryUseCase _searchProducts;
    private readonly CreateProductCommandUseCase _createProduct;
    private readonly UpdateProductCommandUseCase _updateProduct;
    private readonly DeleteProductCommandUseCase _deleteProduct;

    public ProductsController(
        GetAllProductsQueryUseCase getAllProducts,
        GetProductByIdQueryUseCase getProductById,
        GetPagedProductsQueryUseCase getPagedProducts,
        SearchProductsQueryUseCase searchProducts,
        CreateProductCommandUseCase createProduct,
        UpdateProductCommandUseCase updateProduct,
        DeleteProductCommandUseCase deleteProduct)
    {
        _getAllProducts = getAllProducts;
        _getProductById = getProductById;
        _getPagedProducts = getPagedProducts;
        _searchProducts = searchProducts;
        _createProduct = createProduct;
        _updateProduct = updateProduct;
        _deleteProduct = deleteProduct;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll(CancellationToken ct)
    {
        var products = await _getAllProducts.ExecuteAsync(new GetAllProductsQuery(), ct);
        return Ok(products.Select(ProductMapper.ToDto));
    }

    [HttpGet("paged")]
    public async Task<ActionResult<PagedResponse<ProductDto>>> GetPaged(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken ct = default)
    {
        var paged = await _getPagedProducts.ExecuteAsync(
            new GetPagedProductsQuery(page, pageSize),
            ct);

        return Ok(ProductMapper.ToPagedResponse(paged));
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<ProductDto>>> Search(
        [FromQuery] string term,
        CancellationToken ct)
    {
        var products = await _searchProducts.ExecuteAsync(
            new SearchProductsQuery(term),
            ct);

        return Ok(products.Select(ProductMapper.ToDto));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProductDto>> GetById(Guid id, CancellationToken ct)
    {
        var product = await _getProductById.ExecuteAsync(
            new GetProductByIdQuery(id),
            ct);

        return Ok(ProductMapper.ToDto(product));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<ProductDto>> Create([FromBody] CreateProductRequest request, CancellationToken ct)
    {
        var product = await _createProduct.ExecuteAsync(
            new CreateProductCommand(
                request.Name,
                request.Description,
                request.Price,
                request.Stock,
                request.CategoryId),
            ct);

        return CreatedAtAction(
            nameof(GetById),
            new { id = product.Id },
            ProductMapper.ToDto(product));
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductRequest request, CancellationToken ct)
    {
        await _updateProduct.ExecuteAsync(
            new UpdateProductCommand(
                id,
                request.Name,
                request.Description,
                request.Price,
                request.Stock,
                request.CategoryId),
            ct);

        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await _deleteProduct.ExecuteAsync(new DeleteProductCommand(id), ct);
        return NoContent();
    }
}
