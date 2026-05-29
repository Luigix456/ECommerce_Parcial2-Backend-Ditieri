using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Application.Mappings;
using ECommerce.Application.Queries;
using MediatR;

namespace ECommerce.Application.Handlers;

public class GetAllCategoriesQueryHandler
    : IRequestHandler<GetAllCategoriesQuery, IReadOnlyList<CategoryDto>>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetAllCategoriesQueryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<IReadOnlyList<CategoryDto>> Handle(
        GetAllCategoriesQuery request,
        CancellationToken ct
    )
    {
        var categories = await _categoryRepository.GetAllAsync(ct);
        return categories.Select(CategoryMapper.ToDto).ToList();
    }
}
