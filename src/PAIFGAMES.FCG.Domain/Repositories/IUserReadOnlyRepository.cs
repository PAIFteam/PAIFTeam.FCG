using PAIFGAMES.FCG.Domain.Entities;
using PAIFGAMES.FCG.Domain.Extensions;
using PAIFGAMES.FCG.Domain.Users.DTOs;
using PAIFGAMES.FCG.Domain.Users.Filter;

namespace PAIFGAMES.FCG.Domain.Repositories
{
    public interface IUserReadOnlyRepository
    {
        Task<PagedList<UserDto>> GetFilteredUsersAsyncWithoutLockout(UserFilterModel userFilterModel, PageFilterModel pageFilterModel, CancellationToken cancellationToken);
        Task<List<UserDto>> GetAllUsersAsync(CancellationToken cancellationToken);
        IdentityUserCustom? GetUserByUId(Guid userUId);
        Task<IdentityUserCustom?> GetUserByUIdAsync(Guid userUId, CancellationToken cancellationToken);
    }
}
