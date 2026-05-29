using ECommerce.Application.Commands;
using FluentValidation;

namespace ECommerce.Application.Validators;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("El email es obligatorio.")
            .EmailAddress()
            .WithMessage("El formato del email no es v�lido.");

        RuleFor(x => x.Password).NotEmpty().WithMessage("La contrase�a es obligatoria.");
    }
}
