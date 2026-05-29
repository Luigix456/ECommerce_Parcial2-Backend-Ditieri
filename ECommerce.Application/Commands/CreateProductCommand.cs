using ECommerce.Application.DTOs;
using MediatR;

namespace ECommerce.Application.Commands;

public sealed record CreateProductCommand(
    string Name,
    string Description,
    decimal Price,
    int Stock,
    Guid CategoryId
) : IRequest<ProductDto>;
