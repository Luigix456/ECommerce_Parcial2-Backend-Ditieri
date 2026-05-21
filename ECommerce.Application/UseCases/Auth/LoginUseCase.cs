using ECommerce.Application.Interfaces;

namespace ECommerce.Application.UseCases.Auth;

public class LoginUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;

    public LoginUseCase(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        ITokenService tokenService)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    public async Task<string?> ExecuteAsync(string email, string password, CancellationToken ct = default)
    {
        var user = await _userRepository.GetByEmailAsync(email, ct);
        if (user is null)
            return null;

        if (!_passwordHasher.Verify(password, user.PasswordHash))
            return null;

        return _tokenService.GenerateToken(user);
    }
}
