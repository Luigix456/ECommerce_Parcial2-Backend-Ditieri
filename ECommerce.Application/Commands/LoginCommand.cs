using ECommerce.Application.DTOs;
using MediatR;

namespace ECommerce.Application.Commands;

public sealed record LoginCommand(string Email, string Password) : IRequest<AuthResponseDto>;
