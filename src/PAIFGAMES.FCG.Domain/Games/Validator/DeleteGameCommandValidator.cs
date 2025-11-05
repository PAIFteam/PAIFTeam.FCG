using FluentValidation;
using PAIFGAMES.FCG.Domain.Games.Commands;
using PAIFGAMES.FCG.Domain.Repositories;

namespace PAIFGAMES.FCG.Domain.Games.Validator
{
    public class DeleteGameCommandValidator : AbstractValidator<DeleteGameCommand>
    {
        public DeleteGameCommandValidator(IGameReadOnlyRepository gameReadOnlyRepository)
        {
            RuleFor(x => x.GameUId)
            .Must(x => x != Guid.Empty).WithMessage("O campo GameUId é obrigatório.").WithName("GameUId");
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
