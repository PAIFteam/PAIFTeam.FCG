using MediatR;
using Newtonsoft.Json;
using PAIFGAMES.FCG.Domain.Data;
using PAIFGAMES.FCG.Domain.Entities;
using PAIFGAMES.FCG.Domain.Mediator.Notifications;
using PAIFGAMES.FCG.Domain.Mediator;
using PAIFGAMES.FCG.Domain.Mediator.Messages;
using PAIFGAMES.FCG.Domain.Repositories;

namespace PAIFGAMES.FCG.Domain.Users.Commands
{
    public class DetachGameUserCommand : Command
    {
        [JsonProperty("gameUId")]
        public Guid GameUId { get; set; }

        [JsonProperty("userUId")]
        public Guid UserUId { get; set; }


        private Game _game;
        public Game GetGame() => _game;
        public void SetGame(Game game) => _game = game;


        private IdentityUserCustom _identityUserCustom;
        public IdentityUserCustom GetIdentityUserCustom() => _identityUserCustom;
        public void SetIdentityUserCustom(IdentityUserCustom identityUserCustom) => _identityUserCustom = identityUserCustom;
    }
    public class DetachGameUserCommandHandler : CommandHandler, IRequestHandler<DetachGameUserCommand, bool>
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserReadOnlyRepository _userReadOnlyRepository;
        private readonly IGameReadOnlyRepository _gameReadOnlyRepository;
        private readonly IGameUserRepository _gameUserRepository;

        public DetachGameUserCommandHandler(IMediatorHandler mediator,
            INotificationHandler<DomainNotification> notifications,
            IUnitOfWork uow,
            IUserReadOnlyRepository userReadOnlyRepository,
            IGameReadOnlyRepository gameReadOnlyRepository,
            IGameUserRepository gameUserRepository) : base(mediator, notifications)
        {
            _uow = uow;
            _userReadOnlyRepository = userReadOnlyRepository;
            _gameReadOnlyRepository = gameReadOnlyRepository;
            _gameUserRepository = gameUserRepository;
        }
        public async Task<bool> Handle(DetachGameUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userReadOnlyRepository.GetUserByUIdAsync(request.UserUId, cancellationToken);
                if (user == null)
                {
                    await _notifications.Handle(new DomainNotification("User", "Usuário não encontrado."), cancellationToken);
                    return false;
                }

                var game = await _gameReadOnlyRepository.GetGameByUIdAsync(request.GameUId, cancellationToken);
                if (game == null)
                {
                    await _notifications.Handle(new DomainNotification("Game", "Jogo não encontrado."), cancellationToken);
                    return false;
                }

                await _gameUserRepository.DeleteByUserAndGame(user.Id, game.Id, cancellationToken);

                await _uow.SaveAsync(cancellationToken);
                return true;
            }
            catch (Exception ex)
            {
                await _notifications.Handle(new DomainNotification("Game", $"Falha ao desvincular jogo do usuário: {ex.Message}"), cancellationToken);
                return false;
            }
        }
    }
}
