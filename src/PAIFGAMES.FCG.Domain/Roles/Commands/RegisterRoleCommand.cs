using MediatR;
using PAIFGAMES.FCG.Domain.Mediator.Notifications;
using PAIFGAMES.FCG.Domain.Mediator;
using PAIFGAMES.FCG.Domain.Mediator.Messages;
using Microsoft.AspNetCore.Identity;
using PAIFGAMES.FCG.Domain.Entities;

namespace PAIFGAMES.FCG.Domain.Roles.Commands
{
    public class RegisterRoleCommand : Command
    {
        public string Name { get; set; }

    }
    public class RegisterRoleCommandHandler : CommandHandler, IRequestHandler<RegisterRoleCommand, bool>
    {
        private readonly RoleManager<IdentityRoleCustom> _roleManager;
        public RegisterRoleCommandHandler(IMediatorHandler mediator,
            INotificationHandler<DomainNotification> notifications, RoleManager<IdentityRoleCustom> roleManager) : base(mediator, notifications)
        {
            _roleManager = roleManager;
        }
        public async Task<bool> Handle(RegisterRoleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var sucess = await _roleManager.CreateAsync(new IdentityRoleCustom(request.Name));
                if (sucess.Succeeded)
                    return true;
            }
            catch (Exception)
            {
                await _notifications.Handle(new DomainNotification("Role", "Failed to register role."), cancellationToken);
            }
            return false;
        }
    }
}
