using ECommerce.Api.DTOs;
using ECommerce.Api.Mappers;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoriesController(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAll(CancellationToken ct)
    {
        var categories = await _categoryRepository.GetAllAsync(ct);
        return Ok(categories.Select(CategoryMapper.ToDto));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CategoryDto>> GetById(Guid id, CancellationToken ct)
    {
        var category = await _categoryRepository.GetByIdAsync(id, ct);
        if (category is null)
            throw new NotFoundException("Category", id);

        return Ok(CategoryMapper.ToDto(category));
    }
}
