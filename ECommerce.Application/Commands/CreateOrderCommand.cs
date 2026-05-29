using ECommerce.Application.DTOs;
using MediatR;

namespace ECommerce.Application.Commands;

public sealed record CreateOrderCommand(Guid UserId, List<CreateOrderItemCommand> Items)
    : IRequest<OrderDto>;

public sealed record CreateOrderItemCommand(Guid ProductId, int Quantity);
