using MediatR;
using PAIFGAMES.FCG.Domain.Data;
using PAIFGAMES.FCG.Domain.Mediator.Notifications;
using PAIFGAMES.FCG.Domain.Mediator;
using PAIFGAMES.FCG.Domain.Mediator.Messages;
using PAIFGAMES.FCG.Domain.Entities;
using Newtonsoft.Json;

namespace PAIFGAMES.FCG.Domain.Games.Commands
{
    public class RegisterGameCommand : Command
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("value")]
        public decimal Value { get; set; }
    }
    public class RegisterGameCommandHandler : CommandHandler, IRequestHandler<RegisterGameCommand, bool>
    {
        private readonly IUnitOfWork _uow;

        public RegisterGameCommandHandler(IMediatorHandler mediator,
            INotificationHandler<DomainNotification> notifications,
            IUnitOfWork uow) : base(mediator, notifications)
        {
            _uow = uow;
        }
        public async Task<bool> Handle(RegisterGameCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var game = new Game(request.Name, request.Value);
                _uow.GameRepository.Create(game, cancellationToken);
                await _uow.SaveAsync(cancellationToken);
            }
            catch (Exception)
            {
                await _notifications.Handle(new DomainNotification("Game", "Failed to register game."), cancellationToken);
                return false;
            }
            return true;
        }
    }
}