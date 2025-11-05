using MediatR;
using Newtonsoft.Json;
using PAIFGAMES.FCG.Domain.Data;
using PAIFGAMES.FCG.Domain.Entities;
using PAIFGAMES.FCG.Domain.Mediator;
using PAIFGAMES.FCG.Domain.Mediator.Messages;
using PAIFGAMES.FCG.Domain.Mediator.Notifications;
using PAIFGAMES.FCG.Domain.Repositories;

namespace PAIFGAMES.FCG.Domain.Games.Commands
{
    public class DeleteGameCommand : Command
    {
        public DeleteGameCommand(Guid gameUId)
        {
            GameUId = gameUId;
        }

        [JsonProperty("gameUId")]
        public Guid GameUId { get; set; }

        private Game _game;
        public Game GetGame() => _game;
        public void SetGame(Game game) => _game = game;
    }

    public class DeleteGameCommandHandler : CommandHandler, IRequestHandler<DeleteGameCommand, bool>
    {
        private readonly IGameReadOnlyRepository _gameReadOnlyRepository;
        private readonly IUnitOfWork _uow;

        public DeleteGameCommandHandler(IMediatorHandler mediator,
            INotificationHandler<DomainNotification> notifications,
            IGameReadOnlyRepository gameReadOnlyRepository,
            IUnitOfWork uow) : base(mediator, notifications)
        {
            _gameReadOnlyRepository = gameReadOnlyRepository;
            _uow = uow;
        }
        public async Task<bool> Handle(DeleteGameCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _uow.GameRepository.Delete(request.GetGame(), cancellationToken);
                await _uow.SaveAsync(cancellationToken);
            }
            catch (Exception)
            {
                await _notifications.Handle(new DomainNotification("Game", "Failed to delete game."), cancellationToken);
                return false;
            }
            return true;
        }
    }
}
