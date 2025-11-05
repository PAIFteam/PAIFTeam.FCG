using Microsoft.EntityFrameworkCore.Storage;
using PAIFGAMES.FCG.Domain.Repositories;

namespace PAIFGAMES.FCG.Domain.Data
{
    public interface IUnitOfWork
    {
        IGameRepository GameRepository { get; }
        IDbContextTransaction Begin();
        bool Commit();
        void Save();
        Task SaveAsync(CancellationToken cancellationToken);
        void RejectChanges();
        Task<bool> ExecuteStrategy(IEnumerable<Func<Task>> actions);
    }
}
