using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Application.Mappings;
using ECommerce.Application.Queries;
using ECommerce.Domain.Exceptions;
using MediatR;

namespace ECommerce.Application.Handlers;

public class GetMyOrdersQueryHandler : IRequestHandler<GetMyOrdersQuery, IEnumerable<OrderDto>>
{
    private readonly IOrderRepository _orderRepository;

    public GetMyOrdersQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<IEnumerable<OrderDto>> Handle(
        GetMyOrdersQuery request,
        CancellationToken cancellationToken
    )
    {
        if (request.UserId == Guid.Empty)
            throw new DomainRuleException("El usuario autenticado es obligatorio.");

        var orders = await _orderRepository.GetByUserIdAsync(request.UserId, cancellationToken);
        return orders.Select(OrderMapper.ToDto);
    }
}
