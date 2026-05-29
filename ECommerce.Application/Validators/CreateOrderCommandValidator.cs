using ECommerce.Application.Commands;
using FluentValidation;

namespace ECommerce.Application.Validators;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.Items).NotEmpty().WithMessage("La orden debe tener al menos un producto.");

        RuleForEach(x => x.Items)
            .ChildRules(item =>
            {
                item.RuleFor(x => x.ProductId)
                    .NotEmpty()
                    .WithMessage("El producto es obligatorio.");

                item.RuleFor(x => x.Quantity)
                    .GreaterThan(0)
                    .WithMessage("La cantidad debe ser mayor a cero.");
            });
    }
}
