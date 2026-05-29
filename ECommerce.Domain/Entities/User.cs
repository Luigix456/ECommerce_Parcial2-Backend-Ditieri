using ECommerce.Domain.ValueObjects;

namespace ECommerce.Domain.Entities;

public class User : BaseEntity
{
    public string Email { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string Role { get; private set; } = "User";
    public DateTime CreatedAt { get; private set; }

    private User() { }

    public static User Create(string email, string name, string passwordHash, string role = "User")
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("El email es obligatorio.");

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre es obligatorio.");

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("El password hash es obligatorio.");

        return new User
        {
            Id = Guid.NewGuid(),
            Email = email.Trim().ToLower(),
            Name = name.Trim(),
            PasswordHash = passwordHash,
            Role = string.IsNullOrWhiteSpace(role) ? "User" : role,
            CreatedAt = DateTime.UtcNow,
        };
    }
}
