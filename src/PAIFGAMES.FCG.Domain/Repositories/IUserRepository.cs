using PAIFGAMES.FCG.Domain.Entities;

namespace PAIFGAMES.FCG.Domain.Repositories
{
    public interface IUserRepository
    {
        Task AttachRoleToUserAsync(IdentityUserCustom user, string roleNormalizedName, CancellationToken cancellationToken);
        Task DetachRoleFromUserAsync(IdentityUserCustom user, string roleNormalizedName, CancellationToken cancellationToken);

        Task AttachGameToUserAsync(IdentityUserCustom user, Game game, CancellationToken cancellationToken);
        Task DetachGameFromUserAsync(IdentityUserCustom user, Game game, CancellationToken cancellationToken);
    }
}
