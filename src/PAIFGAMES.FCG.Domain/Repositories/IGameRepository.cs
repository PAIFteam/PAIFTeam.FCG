using PAIFGAMES.FCG.Domain.Entities;

namespace PAIFGAMES.FCG.Domain.Repositories
{
    public interface IGameRepository
    {
        void Create(Game game, CancellationToken cancellationToken);
        void Update(Game game, CancellationToken cancellationToken);
        void Delete(Game game, CancellationToken cancellationToken);
    }
}
