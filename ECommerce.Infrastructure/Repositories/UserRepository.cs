using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _ctx;
    public UserRepository(ApplicationDbContext ctx) => _ctx = ctx;

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _ctx.Users.FirstOrDefaultAsync(u => u.Id == id, ct);

    public async Task<User?> GetByEmailAsync(string email, CancellationToken ct = default)
    {
        var normalized = email.Trim().ToLower();
        return await _ctx.Users.FirstOrDefaultAsync(u => u.Email == normalized, ct);
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken ct = default)
    {
        var normalized = email.Trim().ToLower();
        return await _ctx.Users.AnyAsync(u => u.Email == normalized, ct);
    }

    public async Task AddAsync(User user, CancellationToken ct = default)
    {
        await _ctx.Users.AddAsync(user, ct);
        await _ctx.SaveChangesAsync(ct);
    }
}
