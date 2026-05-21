using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Exceptions;

namespace ECommerce.Application.UseCases.Auth;

public class RegisterUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterUseCase(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<User> ExecuteAsync(string email, string name, string password, CancellationToken ct = default)
    {
        if (await _userRepository.ExistsByEmailAsync(email, ct))
            throw new DomainRuleException("Ya existe un usuario con ese email.");

        var hash = _passwordHasher.Hash(password);
        var user = new User(email, name, hash, "User");
        await _userRepository.AddAsync(user, ct);
        return user;
    }
}
