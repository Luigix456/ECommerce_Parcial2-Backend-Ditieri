using ECommerce.Application.DTOs;
using MediatR;

namespace ECommerce.Application.Queries;

public sealed record GetAllCategoriesQuery : IRequest<IReadOnlyList<CategoryDto>>;
