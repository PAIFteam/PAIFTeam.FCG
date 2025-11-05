using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PAIFGAMES.FCG.Domain.Entities;
using PAIFGAMES.FCG.Domain.Extensions;
using PAIFGAMES.FCG.Domain.Games.Filter;
using PAIFGAMES.FCG.Domain.Repositories;
using PAIFGAMES.FCG.Infra.Context;
using PAIFGAMES.FCG.Infra.Data;
using System.Security;

namespace PAIFGAMES.FCG.Infra.Repositories
{
    public class GameReadOnlyRepository : GenericReadOnlyRepository<Game>, IGameReadOnlyRepository
    {
        public GameReadOnlyRepository(ApplicationDbContext applicationDbContext, ILogger<GameReadOnlyRepository> logger)
            : base(applicationDbContext, logger)
        {
        }

        public IEnumerable<Game> GetGamesByUserUId(Guid userUId)
        {
            return _applicationDbContext.UserGames
                .AsNoTracking()
                .Where(ug => ug.User.UId == userUId)
                .Select(ug => ug.Game)
                .ToList();
        }

        public async Task<IEnumerable<Game>> GetGamesByUserUIdAsync(Guid userUId, CancellationToken cancellationToken)
        {
            return await _applicationDbContext.UserGames
                .AsNoTracking()
                .Where(ug => ug.User.UId == userUId)
                .Select(ug => ug.Game)
                .ToListAsync(cancellationToken);
        }

        public PagedList<Game> GetAllGames(GameFilterModel gameFilterModel, PageFilterModel pageFilterModel)
        {
            var mandatorySearch = gameFilterModel.RequiredParams ?? new List<string>();

            var query = _applicationDbContext.Game.AsNoTracking().AsQueryable();

            if (mandatorySearch.Count > 0)
            {
                query = query.Where(g => mandatorySearch.All(param =>
                    (g.Name + "." + g.Value).Contains(param)));
            }

            if (!string.IsNullOrWhiteSpace(gameFilterModel.Search))
            {
                query = query.Where(g =>
                    (g.Name + "." + g.Value).Contains(gameFilterModel.Search));
            }

            var totalCount = query.Count();
            var items = query
                .Skip((pageFilterModel.page - 1) * pageFilterModel.pageSize)
                .Take(pageFilterModel.pageSize)
                .ToList();

            return new PagedList<Game>(items, totalCount, pageFilterModel.page, pageFilterModel.pageSize);
        }

        public async Task<PagedList<Game>> GetAllGamesAsync(GameFilterModel gameFilterModel, PageFilterModel pageFilterModel, CancellationToken cancellationToken)
        {
            var mandatorySearch = gameFilterModel.RequiredParams ?? new List<string>();

            var query = _applicationDbContext.Game.AsNoTracking().AsQueryable();

            if (mandatorySearch.Count > 0)
            {
                query = query.Where(g => mandatorySearch.All(param =>
                    (g.Name + "." + g.Value).Contains(param)));
            }

            if (!string.IsNullOrWhiteSpace(gameFilterModel.Search))
            {
                query = query.Where(g =>
                    (g.Name + "." + g.Value).Contains(gameFilterModel.Search));
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .Skip((pageFilterModel.page - 1) * pageFilterModel.pageSize)
                .Take(pageFilterModel.pageSize)
                .ToListAsync(cancellationToken);

            return new PagedList<Game>(items, totalCount, pageFilterModel.page, pageFilterModel.pageSize);
        }

        public Game? GetGameByUId(Guid gameUId)
        {
            return _applicationDbContext.Game?
                .AsNoTracking()
                .FirstOrDefault(g => g.UId == gameUId);
        }

        public async Task<Game?> GetGameByUIdAsync(Guid gameUId, CancellationToken cancellationToken)
        {
            return await _applicationDbContext.Game
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.UId == gameUId, cancellationToken);
        }
    }
}
