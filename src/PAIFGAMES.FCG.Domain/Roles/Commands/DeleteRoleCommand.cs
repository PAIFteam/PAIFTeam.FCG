using MediatR;
using Microsoft.AspNetCore.Identity;
using PAIFGAMES.FCG.Domain.Entities;
using PAIFGAMES.FCG.Domain.Mediator;
using PAIFGAMES.FCG.Domain.Mediator.Messages;
using PAIFGAMES.FCG.Domain.Mediator.Notifications;

namespace PAIFGAMES.FCG.Domain.Roles.Commands
{
    public class DeleteRoleCommand : Command
    {
        public DeleteRoleCommand(Guid roleUId)
        {
            RoleUId = roleUId;
        }
        public Guid RoleUId { get; set; }

        private IdentityRoleCustom _role;
        public IdentityRoleCustom GetIdentityRoleCustom() => _role;
        public void SetIdentityRoleCustom(IdentityRoleCustom identityRoleCustom) => _role = identityRoleCustom;
    }

    public class DeleteRoleCommandHandler : CommandHandler, IRequestHandler<DeleteRoleCommand, bool>
    {
        private readonly RoleManager<IdentityRoleCustom> _roleManager;

        public DeleteRoleCommandHandler(IMediatorHandler mediator,
            INotificationHandler<DomainNotification> notifications, RoleManager<IdentityRoleCustom> roleManager) : base(mediator, notifications)
        {
            _roleManager = roleManager;
        }
        public async Task<bool> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _roleManager.DeleteAsync(request.GetIdentityRoleCustom());
            }
            catch (Exception)
            {
                await _notifications.Handle(new DomainNotification("Role", "Failed to delete role."), cancellationToken);
                return false;
            }
            return true;
        }
    }
}
