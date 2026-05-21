using ECommerce.Api.DTOs;
using ECommerce.Api.Mappers;
using ECommerce.Application.UseCases.Auth;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly RegisterUseCase _registerUseCase;
    private readonly LoginUseCase _loginUseCase;

    public AuthController(RegisterUseCase registerUseCase, LoginUseCase loginUseCase)
    {
        _registerUseCase = registerUseCase;
        _loginUseCase = loginUseCase;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register([FromBody] RegisterRequest request, CancellationToken ct)
    {
        var user = await _registerUseCase.ExecuteAsync(request.Email, request.Name, request.Password, ct);
        return Created(string.Empty, UserMapper.ToDto(user));
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request, CancellationToken ct)
    {
        var token = await _loginUseCase.ExecuteAsync(request.Email, request.Password, ct);

        if (token is null)
            return Unauthorized(new { message = "Credenciales incorrectas." });

        return Ok(new AuthResponse { Token = token });
    }
}
