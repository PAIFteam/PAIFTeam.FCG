using MediatR;
using PAIFGAMES.FCG.Domain.Mediator.Notifications;
using PAIFGAMES.FCG.Domain.Mediator;
using PAIFGAMES.FCG.Domain.Mediator.Messages;
using Microsoft.AspNetCore.Identity;
using PAIFGAMES.FCG.Domain.Entities;

namespace PAIFGAMES.FCG.Domain.Roles.Commands
{
    public class UpdateRoleCommand : Command
    {
        public Guid RoleUId { get; set; }
        public string Name { get; set; }

        private IdentityRoleCustom _role;
        public IdentityRoleCustom GetIdentityRoleCustom() => _role;
        public void SetIdentityRoleCustom(IdentityRoleCustom identityRoleCustom) => _role = identityRoleCustom;
    }

    public class UpdateRoleCommandHandler : CommandHandler, IRequestHandler<UpdateRoleCommand, bool>
    {
        private readonly RoleManager<IdentityRoleCustom> _roleManager;
        public UpdateRoleCommandHandler(IMediatorHandler mediator,
            INotificationHandler<DomainNotification> notifications,
            RoleManager<IdentityRoleCustom> roleManager
            ) : base(mediator, notifications)
        {
            _roleManager = roleManager;
        }
        public async Task<bool> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var role = request.GetIdentityRoleCustom();
                role.Name = request.Name;
                var result = await _roleManager.UpdateAsync(role.UpdateAt());
                if (result.Succeeded)
                    return true;
            }
            catch (Exception)
            {
                await _notifications.Handle(new DomainNotification("Role", "Failed to update role."), cancellationToken);
                return false;
            }
            return false;
        }
    }
}