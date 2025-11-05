using FluentValidation;
using PAIFGAMES.FCG.Domain.Roles.Commands;

namespace PAIFGAMES.FCG.Domain.Roles.Validator
{
    public class RegisterRoleCommandValidator : AbstractValidator<RegisterRoleCommand>
    {
        public RegisterRoleCommandValidator()
        {
            RuleFor(x => x.Name)
            .Must(x => !string.IsNullOrEmpty(x)).WithMessage("O campo Name é obrigatório.").WithName("Name");
        }
    }
}
