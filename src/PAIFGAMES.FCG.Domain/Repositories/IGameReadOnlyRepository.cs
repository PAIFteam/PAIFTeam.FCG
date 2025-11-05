using PAIFGAMES.FCG.Domain.Entities;
using PAIFGAMES.FCG.Domain.Extensions;
using PAIFGAMES.FCG.Domain.Games.Filter;

namespace PAIFGAMES.FCG.Domain.Repositories
{
    public interface IGameReadOnlyRepository
    {
        IEnumerable<Game> GetGamesByUserUId(Guid userUId);
        Task<IEnumerable<Game>> GetGamesByUserUIdAsync(Guid userUId, CancellationToken cancellationToken);

        PagedList<Game> GetAllGames(GameFilterModel gameFilterModel, PageFilterModel pageFilterModel);
        Task<PagedList<Game>> GetAllGamesAsync(GameFilterModel gameFilterModel, PageFilterModel pageFilterModel, CancellationToken cancellationToken);

        Game? GetGameByUId(Guid gameUId);
        Task<Game?> GetGameByUIdAsync(Guid gameUId, CancellationToken cancellationToken);
    }
}
