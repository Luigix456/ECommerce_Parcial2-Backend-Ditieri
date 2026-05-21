using ECommerce.Domain.Exceptions;

namespace ECommerce.Domain.Entities;

public class User : BaseEntity
{
    public string Email { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string Role { get; private set; } = "User";
    public DateTime CreatedAt { get; private set; }

    private User() { }

    public User(string email, string name, string passwordHash, string role = "User")
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new DomainRuleException("El email es obligatorio.");
        if (!email.Contains('@'))
            throw new DomainRuleException("El email no tiene formato válido.");
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainRuleException("El nombre es obligatorio.");
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new DomainRuleException("El password hash es obligatorio.");
        if (role is not "User" and not "Admin")
            throw new DomainRuleException("El rol debe ser User o Admin.");

        Email = email.Trim().ToLower();
        Name = name.Trim();
        PasswordHash = passwordHash;
        Role = role;
        CreatedAt = DateTime.UtcNow;
    }

    public void ChangeRole(string role)
    {
        if (role is not "User" and not "Admin")
            throw new DomainRuleException("El rol debe ser User o Admin.");
        Role = role;
    }
}
