using ECommerce.Api.DTOs;
using ECommerce.Domain.Entities;

namespace ECommerce.Api.Mappers;

public static class UserMapper
{
    public static UserDto ToDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            Name = user.Name,
            Role = user.Role
        };
    }
}
