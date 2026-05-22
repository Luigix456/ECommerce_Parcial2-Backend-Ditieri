namespace ECommerce.Application.UseCases.Products.Queries;

public sealed record GetPagedProductsQuery(int Page, int PageSize);
