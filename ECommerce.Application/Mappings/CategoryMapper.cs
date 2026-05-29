using ECommerce.Application.DTOs;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Mappings;

public static class CategoryMapper
{
    public static CategoryDto ToDto(Category c) => new() { Id = c.Id, Name = c.Name };
}
