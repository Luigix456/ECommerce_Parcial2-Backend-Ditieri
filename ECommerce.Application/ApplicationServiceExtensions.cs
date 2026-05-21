using ECommerce.Application.UseCases.Auth;
using ECommerce.Application.UseCases.Orders;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Application;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<LoginUseCase>();
        services.AddScoped<RegisterUseCase>();
        services.AddScoped<CreateOrderUseCase>();
        return services;
    }
}
