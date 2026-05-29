using ECommerce.Domain.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Middleware;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext ctx, Exception ex, CancellationToken ct)
    {
        _logger.LogError(ex, "Unhandled exception");
        var (status, title) = ex switch
        {
            NotFoundException e => (StatusCodes.Status404NotFound, e.Message),
            ValidationException => (StatusCodes.Status400BadRequest, "Validation failed"),
            DomainException e => (StatusCodes.Status422UnprocessableEntity, e.Message),
            _ => (StatusCodes.Status500InternalServerError, "Unexpected error"),
        };
        ctx.Response.StatusCode = status;
        await ctx.Response.WriteAsJsonAsync(
            new ProblemDetails
            {
                Status = status,
                Title = title,
                Instance = ctx.Request.Path,
            },
            ct
        );
        return true;
    }
}
