using ECommerce.Api.DTOs;
using ECommerce.Domain.Entities;

namespace ECommerce.Api.Mappers;

public static class CategoryMapper
{
    public static CategoryDto ToDto(Category category)
    {
        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name
        };
    }
}
