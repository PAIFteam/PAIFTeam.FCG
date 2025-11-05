using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PAIFGAMES.FCG.Domain.Entities;
using PAIFGAMES.FCG.Domain.Roles.DTOs;

namespace PAIFGAMES.FCG.Domain.Roles.Queries
{
    public class GetAllRolesQuery : IRequest<List<RoleDto>>
    {
    }

    public class GetAllRolesQueryHandler : IRequestHandler<GetAllRolesQuery, List<RoleDto>>
    {
        private readonly RoleManager<IdentityRoleCustom> _roleManager;
        public GetAllRolesQueryHandler(RoleManager<IdentityRoleCustom> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<List<RoleDto>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
        {
            var roles = await _roleManager.Roles.ToListAsync();
            var rolesDto = new List<RoleDto>();

            foreach (var role in roles)
            {
                rolesDto.Add(new RoleDto
                {
                    UId = role.UId,
                    Name = role.Name,
                    ConcurrencyStamp = role.ConcurrencyStamp,
                    CreatedAt = role.CreatedAt,
                    UpdatedAt = role.UpdatedAt
                });
            }
            return rolesDto;
        }
    }
}

