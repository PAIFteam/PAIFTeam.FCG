using FluentValidation;
using PAIFGAMES.FCG.Domain.Games.Commands;
using PAIFGAMES.FCG.Domain.Repositories;

namespace PAIFGAMES.FCG.Domain.Games.Validator
{
    public class UpdateGameCommandValidator : AbstractValidator<UpdateGameCommand>
    {
        public UpdateGameCommandValidator(IGameReadOnlyRepository gameReadOnlyRepository)
        {
            RuleFor(x => x.Name)
                .Must(x => !string.IsNullOrEmpty(x)).WithMessage("O campo Name é obrigatório.").WithName("Name");

            RuleFor(x => x.GameUId)
                .Must(x => x != Guid.Empty).WithMessage("O campo GameUId é obrigatório.");

            RuleFor(x => x.Value)
                .GreaterThan(0).WithMessage("O campo Value deve ser maior que zero.").WithName("Value");

            When(x => x.GameUId != Guid.Empty, () =>
            {
                RuleFor(x => x)
                    .Cascade(CascadeMode.Stop)
                    .Must(a =>
                    {
                        var game = gameReadOnlyRepository.GetGameByUId(a.GameUId);
                        if (game != null)
                        {
                            a.SetGame(game);
                            return true;
                        }
                        return false;
                    }).WithMessage("Jogo não encontrado.").WithName("GameUId");
            });
        }
    }
}
