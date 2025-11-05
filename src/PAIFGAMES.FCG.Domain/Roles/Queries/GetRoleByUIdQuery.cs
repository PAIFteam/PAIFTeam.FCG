using MediatR;
using Microsoft.AspNetCore.Identity;
using PAIFGAMES.FCG.Domain.Entities;
using PAIFGAMES.FCG.Domain.Mediator.Notifications;
using PAIFGAMES.FCG.Domain.Roles.DTOs;

namespace PAIFGAMES.FCG.Domain.Roles.Queries
{
    public class GetRoleByUIdQuery : IRequest<RoleDto>
    {
        public GetRoleByUIdQuery(Guid roleUId)
        {
            RoleUId = roleUId;
        }
        public Guid RoleUId { get; private set; }

        private IdentityRoleCustom _role;
        public IdentityRoleCustom GetIdentityRoleCustom() => _role;
        public void SetIdentityRoleCustom(IdentityRoleCustom identityRoleCustom) => _role = identityRoleCustom;

    }

    public class GetRoleByUIdQueryHandler : IRequestHandler<GetRoleByUIdQuery, RoleDto>
    {
        private readonly RoleManager<IdentityRoleCustom> _roleManager;
        private readonly INotificationHandler<DomainNotification> _notifications;

        public GetRoleByUIdQueryHandler(RoleManager<IdentityRoleCustom> roleManager, INotificationHandler<DomainNotification> notifications)
        {
            _roleManager = roleManager;
            _notifications = notifications;
        }

        public async Task<RoleDto?> Handle(GetRoleByUIdQuery request, CancellationToken cancellationToken)
        {
            var role = request.GetIdentityRoleCustom();

            var rolePermissions = await _roleManager.GetClaimsAsync(role);

            var roleDto = new RoleDto();
            if (role != null)
            {
                roleDto.UId = role.UId;
                roleDto.Name = role.Name;
                roleDto.ConcurrencyStamp = role.ConcurrencyStamp;
                roleDto.CreatedAt = role.CreatedAt;
                roleDto.UpdatedAt = role.UpdatedAt;
            }
            return roleDto;
        }
    }
}
