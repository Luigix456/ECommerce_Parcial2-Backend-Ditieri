using ECommerce.Application.UseCases.Auth;
using ECommerce.Application.UseCases.Orders;
using ECommerce.Application.UseCases.Products.Commands;
using ECommerce.Application.UseCases.Products.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Application;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Auth use cases
        services.AddScoped<LoginUseCase>();
        services.AddScoped<RegisterUseCase>();

        // Order command use cases
        services.AddScoped<CreateOrderUseCase>();

        // Product query use cases
        services.AddScoped<GetAllProductsQueryUseCase>();
        services.AddScoped<GetProductByIdQueryUseCase>();
        services.AddScoped<GetPagedProductsQueryUseCase>();
        services.AddScoped<SearchProductsQueryUseCase>();

        // Product command use cases
        services.AddScoped<CreateProductCommandUseCase>();
        services.AddScoped<UpdateProductCommandUseCase>();
        services.AddScoped<DeleteProductCommandUseCase>();

        return services;
    }
}
