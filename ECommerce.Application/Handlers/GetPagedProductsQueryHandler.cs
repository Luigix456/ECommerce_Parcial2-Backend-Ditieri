using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Application.Mappings;
using ECommerce.Application.Queries;
using MediatR;

namespace ECommerce.Application.Handlers;

public class GetPagedProductsQueryHandler
    : IRequestHandler<GetPagedProductsQuery, PagedResponse<ProductDto>>
{
    private readonly IProductRepository _productRepository;

    public GetPagedProductsQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<PagedResponse<ProductDto>> Handle(
        GetPagedProductsQuery request,
        CancellationToken cancellationToken
    )
    {
        var page = request.Page <= 0 ? 1 : request.Page;
        var pageSize = request.PageSize <= 0 ? 10 : request.PageSize;

        var pagedProducts = await _productRepository.GetPagedAsync(
            page,
            pageSize,
            cancellationToken
        );

        return new PagedResponse<ProductDto>
        {
            Items = pagedProducts.Items.Select(ProductMapper.ToDto).ToList(),

            TotalCount = pagedProducts.TotalCount,
            TotalPages = pagedProducts.TotalPages,
            CurrentPage = pagedProducts.CurrentPage,
            PageSize = pagedProducts.PageSize,
        };
    }
}
