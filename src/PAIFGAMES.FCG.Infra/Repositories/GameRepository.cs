using Microsoft.Extensions.Logging;
using PAIFGAMES.FCG.Domain.Entities;
using PAIFGAMES.FCG.Domain.Repositories;
using PAIFGAMES.FCG.Infra.Context;
using PAIFGAMES.FCG.Infra.Data;

namespace PAIFGAMES.FCG.Infra.Repositories
{
    public class GameRepository : GenericRepository<Game>, IGameRepository
    {
        public GameRepository(ApplicationDbContext applicationDbContext, ILogger<GameRepository> logger)
            : base(applicationDbContext, logger)
        {
        }

        public void Create(Game game, CancellationToken cancellationToken)
        {
            DbSet.Add(game);
        }

        public void Delete(Game game, CancellationToken cancellationToken)
        {
            DbSet.Remove(game);
        }

        public void Update(Game game, CancellationToken cancellationToken)
        {
            DbSet.Update(game);
        }
    }
}
