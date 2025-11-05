using FluentValidation;
using PAIFGAMES.FCG.Domain.Users.Commands;


namespace PAIFGAMES.FCG.Domain.Users.Validator
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("O campo Email é obrigatório.")
                .EmailAddress().WithMessage("O campo Email deve ser um endereço de e-mail válido.");

            RuleFor(x => x.FullName)
            .Must(x => !string.IsNullOrEmpty(x)).WithMessage("O campo FullName é obrigatório.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("O campo Senha é obrigatório.")
                .MinimumLength(6).WithMessage("A senha deve ter no mínimo 6 caracteres.")
                .Must(p => p.Any(char.IsUpper)).WithMessage("A senha deve conter pelo menos uma letra maiúscula.")
                .Must(p => p.Any(char.IsLower)).WithMessage("A senha deve conter pelo menos uma letra minúscula.")
                .Must(p => p.Any(char.IsDigit)).WithMessage("A senha deve conter pelo menos um número.")
                .Must(p => p.Any(c => !char.IsLetterOrDigit(c))).WithMessage("A senha deve conter pelo menos um caractere especial.");
        }
    }
}
