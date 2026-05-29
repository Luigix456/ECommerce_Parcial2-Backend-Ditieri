using ECommerce.Application.Commands;
using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Application.Mappings;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Exceptions;
using MediatR;

namespace ECommerce.Application.Handlers;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderDto>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;

    public CreateOrderCommandHandler(
        IOrderRepository orderRepository,
        IProductRepository productRepository
    )
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
    }

    public async Task<OrderDto> Handle(
        CreateOrderCommand request,
        CancellationToken cancellationToken
    )
    {
        if (request.UserId == Guid.Empty)
            throw new DomainRuleException("El usuario autenticado es obligatorio.");

        if (request.Items is null || !request.Items.Any())
            throw new DomainRuleException("La orden debe contener al menos un producto.");

        var order = Order.Create(request.UserId);

        foreach (var item in request.Items)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId, cancellationToken);

            if (product is null)
                throw new NotFoundException("Product", item.ProductId);

            order.AddItem(product, item.Quantity);
        }

        await _orderRepository.AddAsync(order, cancellationToken);

        return OrderMapper.ToDto(order);
    }
}
