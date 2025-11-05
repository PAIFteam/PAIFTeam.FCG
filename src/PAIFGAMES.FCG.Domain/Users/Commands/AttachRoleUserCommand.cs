using MediatR;
using Microsoft.AspNetCore.Identity;
using PAIFGAMES.FCG.Domain.Data;
using PAIFGAMES.FCG.Domain.Entities;
using PAIFGAMES.FCG.Domain.Mediator;
using PAIFGAMES.FCG.Domain.Mediator.Messages;
using PAIFGAMES.FCG.Domain.Mediator.Notifications;

namespace PAIFGAMES.FCG.Domain.Users.Commands
{
    public class AttachRoleUserCommand : Command
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
    public class AttachRoleUserCommandHandler : CommandHandler, IRequestHandler<AttachRoleUserCommand, bool>
    {

        private readonly UserManager<IdentityUserCustom> _userManager;
        private readonly RoleManager<IdentityRoleCustom> _roleManager;
        private readonly IUnitOfWork _uow;


        public AttachRoleUserCommandHandler(IMediatorHandler mediator,
            INotificationHandler<DomainNotification> notifications,
            UserManager<IdentityUserCustom> userManager,
            RoleManager<IdentityRoleCustom> roleManager,
            IUnitOfWork uow) : base(mediator, notifications)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _uow = uow;
        }
        public async Task<bool> Handle(AttachRoleUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = request.GetIdentityUserCustom();
                var role = request.GetIdentityRoleCustom().NormalizedName!;

                var actions = new Func<Task>[]
                {
                async () => {await _userManager.AddToRoleAsync(user, role); },
                async () => {await _userManager.UpdateAsync(user.UpdateAt()); }
                };

                var success = await _uow.ExecuteStrategy(actions);

                if (success)
                    return true;
                else
                    await _notifications.Handle(new DomainNotification("AttachRole", "Ocorreu um erro ao atribuir um perfil."), cancellationToken);

            }
            catch (Exception)
            {
                await _notifications.Handle(new DomainNotification("AttachRole", "Ocorreu um erro ao atribuir um perfil."), cancellationToken);
            }
                return false;
        }
    }
}
