using Microsoft.EntityFrameworkCore;
using PAIFGAMES.FCG.Domain.Entities;
using PAIFGAMES.FCG.Domain.Extensions;
using PAIFGAMES.FCG.Domain.Repositories;
using PAIFGAMES.FCG.Domain.Users.DTOs;
using PAIFGAMES.FCG.Domain.Users.Filter;
using PAIFGAMES.FCG.Infra.Context;

namespace PAIFGAMES.FCG.Infra.Repositories
{
    public class UserReadOnlyRepository : IUserReadOnlyRepository
    {
        private readonly ApplicationDbContext _context;

        public UserReadOnlyRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedList<UserDto>> GetFilteredUsersAsyncWithoutLockout(UserFilterModel userFilterModel, PageFilterModel pageFilterModel, CancellationToken cancellationToken)
        {
            var roleGuids = userFilterModel.RoleIds ?? Enumerable.Empty<Guid>();
            var roleCount = roleGuids == Enumerable.Empty<Guid>() ? 0 : roleGuids.Count();

            var filteredUsers = await (from u in _context.Users
                                       where (u.FullName != null && u.FullName.Contains(userFilterModel.Search)) ||
                                       (u.Email != null && u.Email.Contains(userFilterModel.Search))
                                       join ur in _context.UserRoles on u.Id equals ur.UserId into userRoles
                                       from ur in userRoles.DefaultIfEmpty()
                                       join r in _context.Roles on ur.RoleId equals r.Id into roles
                                       from r in roles.DefaultIfEmpty()
                                       group new { u, r } by new
                                       {
                                           u.UId,
                                           u.FullName,
                                           u.Email,
                                           u.UserName,
                                           u.LockoutEnd,
                                           u.LockoutEnabled,
                                           u.CreatedAt,
                                           u.UpdatedAt
                                       } into grouped
                                       where roleGuids.Any() ? grouped.Count(x => roleGuids.Contains(x.r.UId)) == roleCount : true
                                       select new UserDto
                                       {
                                           UId = grouped.Key.UId,
                                           FullName = grouped.Key.FullName,
                                           UserName = grouped.Key.UserName,
                                           Email = grouped.Key.Email,
                                           LockoutEnd = grouped.Key.LockoutEnd,
                                           LockoutEnabled = grouped.Key.LockoutEnabled,
                                           CreatedAt = grouped.Key.CreatedAt,
                                           UpdatedAt = grouped.Key.UpdatedAt,
                                           Roles = grouped.Any(x => x.r != null) ? grouped.Where(x => x.r != null).Select(x => new RoleInfo { RoleName = x.r.NormalizedName, RoleUId = x.r.UId }) : null
                                       })
                                 .OrderBy(x => x.FullName)
                                 .Where(x => x.LockoutEnd == null || x.LockoutEnd <= DateTimeOffset.Now)
                                 .PagingAsync(pageFilterModel.pageSize, pageFilterModel.page);

            return filteredUsers;
        }

        public async Task<List<UserDto>> GetAllUsersAsync(CancellationToken cancellationToken)
        {
            var users = await (from u in _context.Users
                               join ur in _context.UserRoles on u.Id equals ur.UserId into userRoles
                               from ur in userRoles.DefaultIfEmpty()
                               join r in _context.Roles on ur.RoleId equals r.Id into roles
                               from r in roles.DefaultIfEmpty()
                               group new { u, r } by new
                               {
                                   u.UId,
                                   u.FullName,
                                   u.Email,
                                   u.UserName,
                                   u.LockoutEnd,
                                   u.LockoutEnabled,
                                   u.CreatedAt,
                                   u.UpdatedAt
                               } into grouped
                               select new UserDto
                               {
                                   UId = grouped.Key.UId,
                                   FullName = grouped.Key.FullName,
                                   UserName = grouped.Key.UserName,
                                   Email = grouped.Key.Email,
                                   LockoutEnd = grouped.Key.LockoutEnd,
                                   LockoutEnabled = grouped.Key.LockoutEnabled,
                                   CreatedAt = grouped.Key.CreatedAt,
                                   UpdatedAt = grouped.Key.UpdatedAt,
                                   Roles = grouped.Any(x => x.r != null) ? grouped.Where(x => x.r != null).Select(x => new RoleInfo { RoleName = x.r.NormalizedName, RoleUId = x.r.UId }) : null
                               })
                                 .OrderBy(x => x.FullName)
                                 .ToListAsync();
            return users;
        }

        public IdentityUserCustom? GetUserByUId(Guid userUId)
        {
            var user = _context.Users
                .Include(u => u.UserGames)
                    .ThenInclude(ug => ug.Game)
                .FirstOrDefault(u => u.UId == userUId);

            return user;
        }

        public async Task<IdentityUserCustom?> GetUserByUIdAsync(Guid userUId, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .Include(u => u.UserGames)
                    .ThenInclude(ug => ug.Game)
                .FirstOrDefaultAsync(u => u.UId == userUId, cancellationToken);

            return user;
        }
    }
}
