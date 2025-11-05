using FluentValidation;
using PAIFGAMES.FCG.Domain.Games.Commands;

namespace PAIFGAMES.FCG.Domain.Games.Validator
{
    public class RegisterGameCommandValidator : AbstractValidator<RegisterGameCommand>
    {
        public RegisterGameCommandValidator()
        {
            RuleFor(x => x.Name)
                .Must(x => !string.IsNullOrEmpty(x))
                .WithMessage("O campo Name é obrigatório.")
                .WithName("Name");

            RuleFor(x => x.Value)
                .GreaterThan(0)
                .WithMessage("O campo Value deve ser maior que zero.")
                .WithName("Value");
        }
    }
}
