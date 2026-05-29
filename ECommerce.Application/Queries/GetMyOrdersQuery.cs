using ECommerce.Application.DTOs;
using MediatR;

namespace ECommerce.Application.Queries;

public sealed record GetMyOrdersQuery(Guid UserId) : IRequest<IEnumerable<OrderDto>>;
