using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Infrastructure.Persistence;

public static class DataSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

        await context.Database.MigrateAsync();

        if (!await context.Users.AnyAsync(u => u.Email == "admin@test.com"))
        {
            var admin = User.Create(
                "admin@test.com",
                "Administrador",
                passwordHasher.Hash("Admin123!"),
                "Admin"
            );

            await context.Users.AddAsync(admin);
        }

        if (!await context.Users.AnyAsync(u => u.Email == "user@test.com"))
        {
            var user = User.Create(
                "user@test.com",
                "Usuario Demo",
                passwordHasher.Hash("User123!"),
                "User"
            );

            await context.Users.AddAsync(user);
        }

        await context.SaveChangesAsync();
    }
}
