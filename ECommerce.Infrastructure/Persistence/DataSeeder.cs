using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Persistence.Configurations;
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
            var admin = new User(
                "admin@test.com",
                "Administrador",
                passwordHasher.Hash("Admin123!"),
                "Admin");
            await context.Users.AddAsync(admin);
        }

        if (!await context.Users.AnyAsync(u => u.Email == "user@test.com"))
        {
            var user = new User(
                "user@test.com",
                "Usuario Demo",
                passwordHasher.Hash("User123!"),
                "User");
            await context.Users.AddAsync(user);
        }

        if (!await context.Products.AnyAsync())
        {
            var products = new[]
            {
                Product.Create("Notebook Lenovo", "Notebook para estudio y trabajo", 950000m, 10, CategoryConfiguration.ElectronicaId),
                Product.Create("Mouse Logitech", "Mouse inalámbrico", 25000m, 30, CategoryConfiguration.ElectronicaId),
                Product.Create("Remera básica", "Remera de algodón", 12000m, 50, CategoryConfiguration.RopaId),
                Product.Create("Silla gamer", "Silla ergonómica", 210000m, 5, CategoryConfiguration.HogarId)
            };
            await context.Products.AddRangeAsync(products);
        }

        await context.SaveChangesAsync();
    }
}
