using MediatR;
using Microsoft.AspNetCore.Identity;
using PAIFGAMES.FCG.Domain.Entities;
using PAIFGAMES.FCG.Domain.Games.DTOs;
using PAIFGAMES.FCG.Domain.Mediator.Notifications;
using PAIFGAMES.FCG.Domain.Repositories;
using PAIFGAMES.FCG.Domain.Users.DTOs;

namespace PAIFGAMES.FCG.Domain.Users.Queries
{
    public class GetUserByUIdQuery : IRequest<UserDto>
    {
        public GetUserByUIdQuery(Guid userUId)
        {
            UserUId = userUId;
        }
        public Guid UserUId { get; private set; }

        private IdentityUserCustom _user;
        public IdentityUserCustom GetIdentityUserCustom() => _user;
        public void SetIdentityUserCustom(IdentityUserCustom identityUserCustom) => _user = identityUserCustom;
    }

    public class GetUserByUIdQueryHandler : IRequestHandler<GetUserByUIdQuery, UserDto>
    {
        private readonly UserManager<IdentityUserCustom> _userManager;
        private readonly RoleManager<IdentityRoleCustom> _roleManager;
        private readonly INotificationHandler<DomainNotification> _notifications;
        private readonly IUserReadOnlyRepository _userReadOnlyRepository;

        public GetUserByUIdQueryHandler(UserManager<IdentityUserCustom> userManager, RoleManager<IdentityRoleCustom> roleManager, INotificationHandler<DomainNotification> notifications,
          IUserReadOnlyRepository userReadOnlyRepository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _notifications = notifications;
            _userReadOnlyRepository = userReadOnlyRepository;
        }

        public async Task<UserDto?> Handle(GetUserByUIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userReadOnlyRepository.GetUserByUIdAsync(request.UserUId, cancellationToken);
            if (user == null)
                return null;

            var userClaims = await _userManager.GetClaimsAsync(user);
            var userRoles = await _userManager.GetRolesAsync(user);

            var roles = new List<RoleInfo>();
            foreach (var userRole in userRoles)
            {
                var role = await _roleManager.FindByNameAsync(userRole);
                if (role != null)
                {
                    var roleInfo = new RoleInfo()
                    {
                        RoleName = role.Name,
                        RoleUId = role.UId,
                    };
                    roles.Add(roleInfo);
                }
            }

            var games = user.UserGames?
                .Where(ug => ug.Game != null)
                .Select(ug => new GameDto
                {
                    UId = ug.Game.UId,
                    Name = ug.Game.Name,
                    Value = ug.Game.Value.ToString("F2")
                }).ToList();

            var userDto = new UserDto
            {
                UId = user.UId,
                FullName = user.FullName,
                UserName = user.UserName,
                Email = user.Email,
                LockoutEnd = user.LockoutEnd,
                LockoutEnabled = user.LockoutEnabled,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                Roles = roles.Any() ? roles : null,
                Games = games
            };

            return userDto;
        }
    }
}