using PAIFGAMES.FCG.Domain.Entities;

namespace PAIFGAMES.FCG.Domain.Repositories
{
    public interface IGameUserRepository
    {
        Task Create(UserGame userGame, CancellationToken cancellationToken);
        Task DeleteByUserAndGame(long userId, long gameId, CancellationToken cancellationToken);
    }
}
