using ECommerce.Application.DTOs;
using MediatR;

namespace ECommerce.Application.Queries;

public sealed record GetPagedProductsQuery(int Page, int PageSize)
    : IRequest<PagedResponse<ProductDto>>;
