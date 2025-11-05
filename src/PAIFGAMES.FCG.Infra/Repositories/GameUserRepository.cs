using Microsoft.EntityFrameworkCore;
using PAIFGAMES.FCG.Domain.Entities;
using PAIFGAMES.FCG.Domain.Repositories;
using PAIFGAMES.FCG.Infra.Context;

namespace PAIFGAMES.FCG.Infra.Repositories
{
    public class GameUserRepository : IGameUserRepository
    {
        private readonly ApplicationDbContext _context;

        public GameUserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Create(UserGame userGame, CancellationToken cancellationToken)
        {
            await _context.UserGames.AddAsync(userGame, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteByUserAndGame(long userId, long gameId, CancellationToken cancellationToken)
        {
            var entity = await _context.UserGames
                .FirstOrDefaultAsync(ug => ug.UserId == userId && ug.GameId == gameId, cancellationToken);

            if (entity != null)
            {
                _context.UserGames.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
