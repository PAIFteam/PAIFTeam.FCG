using Microsoft.EntityFrameworkCore;
using PAIFGAMES.FCG.Domain.Entities;
using PAIFGAMES.FCG.Domain.Repositories;
using PAIFGAMES.FCG.Infra.Context;

namespace PAIFGAMES.FCG.Infra.Repositories
{
    public class GameUserReadOnlyRepository : IGameUserReadOnlyRepository
    {
        private readonly ApplicationDbContext _context;

        public GameUserReadOnlyRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<UserGame> GetByUserId(long userId)
        {
            return _context.UserGames
                .Include(ug => ug.User)
                .Include(ug => ug.Game)
                .Where(ug => ug.UserId == userId)
                .ToList();
        }

        public UserGame? GetByUserIdAndGameId(long userId, long gameId)
        {
            return _context.UserGames
                .Include(ug => ug.User)
                .Include(ug => ug.Game)
                .FirstOrDefault(ug => ug.UserId == userId && ug.GameId == gameId);
        }

        public async Task<UserGame?> GetByUserIdAndGameIdAsync(long userId, long gameId, CancellationToken cancellationToken)
        {
            return await _context.UserGames
                .Include(ug => ug.User)
                .Include(ug => ug.Game)
                .FirstOrDefaultAsync(ug => ug.UserId == userId && ug.GameId == gameId, cancellationToken);
        }

        public async Task<IEnumerable<UserGame>> GetByUserIdAsync(long userId, CancellationToken cancellationToken)
        {
            return await _context.UserGames
                .Include(ug => ug.User)
                .Include(ug => ug.Game)
                .Where(ug => ug.UserId == userId)
                .ToListAsync(cancellationToken);
        }
    }
}
