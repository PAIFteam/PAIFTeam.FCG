using MediatR;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using PAIFGAMES.FCG.Domain.Entities;
using PAIFGAMES.FCG.Domain.Mediator;
using PAIFGAMES.FCG.Domain.Mediator.Messages;
using PAIFGAMES.FCG.Domain.Mediator.Notifications;

namespace PAIFGAMES.FCG.Domain.Users.Commands
{
    public class RegisterUserCommand : Command
    {
        [JsonProperty("fullName")]
        public string FullName { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }

    }
    public class RegisterUserCommandHandler : CommandHandler, IRequestHandler<RegisterUserCommand, bool>
    {

        private readonly UserManager<IdentityUserCustom> _userManager;

        public RegisterUserCommandHandler(IMediatorHandler mediator,
            INotificationHandler<DomainNotification> notifications,
            UserManager<IdentityUserCustom> userManager) : base(mediator, notifications)
        {
            _userManager = userManager;

        }
        public async Task<bool> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {

            try
            {
                var user = new IdentityUserCustom(request.FullName, request.Email);
                var result = await _userManager.CreateAsync(user, request.Password);
                if (result.Succeeded)
                    return true;
            }
            catch (Exception)
            {
                await _notifications.Handle(new DomainNotification("Register", "Ocorreu um erro ao registrar o usuário."), cancellationToken);
                return false;
            }
            return false;
        }
    }
}
