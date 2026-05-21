using System.Security.Claims;
using ECommerce.Api.DTOs;
using ECommerce.Api.Mappers;
using ECommerce.Application.Interfaces;
using ECommerce.Application.UseCases.Orders;
using ECommerce.Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly CreateOrderUseCase _createOrderUseCase;
    private readonly IOrderRepository _orderRepository;

    public OrdersController(CreateOrderUseCase createOrderUseCase, IOrderRepository orderRepository)
    {
        _createOrderUseCase = createOrderUseCase;
        _orderRepository = orderRepository;
    }

    [HttpPost]
    public async Task<ActionResult<OrderDto>> Create([FromBody] CreateOrderRequest request, CancellationToken ct)
    {
        var userId = GetCurrentUserId();
        var items = request.Items
            .Select(i => new CreateOrderItemInput(i.ProductId, i.Quantity))
            .ToList();

        var order = await _createOrderUseCase.ExecuteAsync(userId, items, ct);
        return CreatedAtAction(nameof(GetById), new { id = order.Id }, OrderMapper.ToDto(order));
    }

    [HttpGet("my-orders")]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetMyOrders(CancellationToken ct)
    {
        var userId = GetCurrentUserId();
        var orders = await _orderRepository.GetByUserIdAsync(userId, ct);
        return Ok(orders.Select(OrderMapper.ToDto));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<OrderDto>> GetById(Guid id, CancellationToken ct)
    {
        var order = await _orderRepository.GetByIdWithItemsAsync(id, ct);
        if (order is null)
            throw new NotFoundException("Order", id);

        var userId = GetCurrentUserId();
        var isAdmin = User.IsInRole("Admin");
        if (!isAdmin && order.UserId != userId)
            return Forbid();

        return Ok(OrderMapper.ToDto(order));
    }

    private Guid GetCurrentUserId()
    {
        var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdValue, out var userId))
            throw new UnauthorizedAccessException();

        return userId;
    }
}
