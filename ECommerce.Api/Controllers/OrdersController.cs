using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using ECommerce.Application.Commands;
using ECommerce.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateOrderRequest request,
        CancellationToken ct
    )
    {
        var userId = GetAuthenticatedUserId();

        var command = new CreateOrderCommand(
            userId,
            request.Items.Select(i => new CreateOrderItemCommand(i.ProductId, i.Quantity)).ToList()
        );

        var result = await _mediator.Send(command, ct);

        return Ok(result);
    }

    [HttpGet("my-orders")]
    public async Task<IActionResult> GetMyOrders(CancellationToken ct)
    {
        var userId = GetAuthenticatedUserId();

        var query = new GetMyOrdersQuery(userId);

        var result = await _mediator.Send(query, ct);

        return Ok(result);
    }

    private Guid GetAuthenticatedUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(userIdClaim))
            throw new UnauthorizedAccessException("Usuario no autenticado.");

        return Guid.Parse(userIdClaim);
    }
}

public class CreateOrderRequest
{
    public List<CreateOrderItemRequest> Items { get; set; } = new();
}

public class CreateOrderItemRequest
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}
