using MediatR;
using PAIFGAMES.FCG.Domain.Data;
using PAIFGAMES.FCG.Domain.Mediator.Notifications;
using PAIFGAMES.FCG.Domain.Mediator;
using PAIFGAMES.FCG.Domain.Mediator.Messages;
using PAIFGAMES.FCG.Domain.Entities;
using PAIFGAMES.FCG.Domain.Repositories;
using Newtonsoft.Json;

namespace PAIFGAMES.FCG.Domain.Games.Commands
{
    public class UpdateGameCommand : Command
    {
        [JsonProperty("gameUId")]
        public Guid GameUId { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("value")]
        public decimal Value { get; set; }

        private Game _game;
        public Game GetGame() => _game;
        public void SetGame(Game game) => _game = game;
    }

    public class UpdateGameCommandHandler : CommandHandler, IRequestHandler<UpdateGameCommand, bool>
    {
        private readonly IGameReadOnlyRepository _gameReadOnlyRepository;
        private readonly IUnitOfWork _uow;

        public UpdateGameCommandHandler(IMediatorHandler mediator,
            INotificationHandler<DomainNotification> notifications,
            IGameReadOnlyRepository gameReadOnlyRepository,
            IUnitOfWork uow) : base(mediator, notifications)
        {
            _gameReadOnlyRepository = gameReadOnlyRepository;
            _uow = uow;
        }
        public async Task<bool> Handle(UpdateGameCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var game = request.GetGame();
                game.Name = request.Name;
                game.Value = request.Value;
                _uow.GameRepository.Update(game, cancellationToken);
                await _uow.SaveAsync(cancellationToken);
            }
            catch (Exception)
            {
                await _notifications.Handle(new DomainNotification("Game", "Failed to update game."), cancellationToken);
                return false;
            }
            return true;
        }
    }
}