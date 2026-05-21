using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Exceptions;

namespace ECommerce.Application.UseCases.Orders;

public record CreateOrderItemInput(Guid ProductId, int Quantity);

public class CreateOrderUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IProductRepository _productRepository;
    private readonly IOrderRepository _orderRepository;

    public CreateOrderUseCase(
        IUserRepository userRepository,
        IProductRepository productRepository,
        IOrderRepository orderRepository)
    {
        _userRepository = userRepository;
        _productRepository = productRepository;
        _orderRepository = orderRepository;
    }

    public async Task<Order> ExecuteAsync(Guid userId, List<CreateOrderItemInput> items, CancellationToken ct = default)
    {
        var user = await _userRepository.GetByIdAsync(userId, ct);
        if (user is null)
            throw new NotFoundException(nameof(User), userId);

        if (items.Count == 0)
            throw new DomainRuleException("La orden debe tener al menos un producto.");

        var order = new Order(userId);

        foreach (var input in items)
        {
            var product = await _productRepository.GetByIdAsync(input.ProductId, ct);
            if (product is null)
                throw new NotFoundException(nameof(Product), input.ProductId);

            order.AddItem(product, input.Quantity);
        }

        await _orderRepository.AddAsync(order, ct);
        return order;
    }
}
