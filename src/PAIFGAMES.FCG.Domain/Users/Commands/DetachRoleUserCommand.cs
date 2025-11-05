using MediatR;
using Microsoft.AspNetCore.Identity;
using PAIFGAMES.FCG.Domain.Entities;
using PAIFGAMES.FCG.Domain.Mediator.Notifications;
using PAIFGAMES.FCG.Domain.Mediator;
using PAIFGAMES.FCG.Domain.Mediator.Messages;
using PAIFGAMES.FCG.Domain.Data;

namespace PAIFGAMES.FCG.Domain.Users.Commands
{
    public class DetachRoleUserCommand : Command
    {
        public Guid UserUId { get; set; }
        public Guid RoleUId { get; set; }

        private IdentityUserCustom _user;
        public IdentityUserCustom GetIdentityUserCustom() => _user;
        public void SetIdentityUserCustom(IdentityUserCustom identityUserCustom) => _user = identityUserCustom;

        private IdentityRoleCustom _role;
        public IdentityRoleCustom GetIdentityRoleCustom() => _role;
        public void SetIdentityRoleCustom(IdentityRoleCustom identityRoleCustom) => _role = identityRoleCustom;

    }
    public class DetachRoleUserCommandHandler : CommandHandler, IRequestHandler<DetachRoleUserCommand, bool>
    {

        private readonly UserManager<IdentityUserCustom> _userManager;
        private readonly RoleManager<IdentityRoleCustom> _roleManager;
        private readonly IUnitOfWork _uow;

        public DetachRoleUserCommandHandler(IMediatorHandler mediator,
            INotificationHandler<DomainNotification> notifications,
            UserManager<IdentityUserCustom> userManager,
            RoleManager<IdentityRoleCustom> roleManager,
            IUnitOfWork uow) : base(mediator, notifications)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _uow = uow;
        }
        public async Task<bool> Handle(DetachRoleUserCommand request, CancellationToken cancellationToken)
        {
            try {
                var user = request.GetIdentityUserCustom();
                var role = request.GetIdentityRoleCustom().NormalizedName!;

                var actions = new Func<Task>[]
                {
                async () => {await _userManager.RemoveFromRoleAsync(user, role); },
                async () => {await _userManager.UpdateAsync(user.UpdateAt()); }
                };

                var success = await _uow.ExecuteStrategy(actions);

                if (success)
                    return true;
                else
                    await _notifications.Handle(new DomainNotification("DetachRole", "Ocorreu um erro ao remover o perfil."), cancellationToken);
               
            }
            catch (Exception)
            {
                await _notifications.Handle(new DomainNotification("DetachRole", "Ocorreu um erro ao remover o perfil."), cancellationToken);
            }
            return false;
        }
    }
}
