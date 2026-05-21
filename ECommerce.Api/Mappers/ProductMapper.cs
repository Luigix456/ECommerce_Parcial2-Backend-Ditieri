using ECommerce.Api.DTOs;
using ECommerce.Application.Common;
using ECommerce.Domain.Entities;

namespace ECommerce.Api.Mappers;

public static class ProductMapper
{
    public static ProductDto ToDto(Product product)
    {
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock,
            CategoryId = product.CategoryId,
            CategoryName = product.Category?.Name ?? string.Empty,
            IsActive = product.IsActive
        };
    }

    public static PagedResponse<ProductDto> ToPagedResponse(PagedResult<Product> paged)
    {
        return new PagedResponse<ProductDto>
        {
            Items = paged.Items.Select(ToDto).ToList(),
            TotalCount = paged.TotalCount,
            TotalPages = paged.TotalPages,
            CurrentPage = paged.CurrentPage,
            PageSize = paged.PageSize
        };
    }
}
