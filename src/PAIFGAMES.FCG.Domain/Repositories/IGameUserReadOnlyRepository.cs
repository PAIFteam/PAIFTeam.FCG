using PAIFGAMES.FCG.Domain.Entities;

namespace PAIFGAMES.FCG.Domain.Repositories
{
    public interface IGameUserReadOnlyRepository
    {
        UserGame? GetByUserIdAndGameId(long userId, long gameId);
        Task<UserGame?> GetByUserIdAndGameIdAsync(long userId, long gameId, CancellationToken cancellationToken);
        IEnumerable<UserGame> GetByUserId(long userId);
        Task<IEnumerable<UserGame>> GetByUserIdAsync(long userId, CancellationToken cancellationToken);
    }
}
