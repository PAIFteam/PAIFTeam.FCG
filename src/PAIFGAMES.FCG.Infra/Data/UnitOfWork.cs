using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using PAIFGAMES.FCG.Infra.Context;
using PAIFGAMES.FCG.Domain.Repositories;
using PAIFGAMES.FCG.Domain.Data;

namespace PAIFGAMES.FCG.Infra.Data
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {

        private bool _disposed = false;
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction _transaction = null;
        public IGameRepository GameRepository { get; private set; }

        public UnitOfWork(
            ApplicationDbContext context,
            IGameRepository gameRepository)
        {
            _context = context;
            GameRepository = gameRepository;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public async Task SaveAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void RejectChanges()
        {
            foreach (var entry in _context.ChangeTracker.Entries()
                  .Where(e => e.State != EntityState.Unchanged))
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                    entry.State = EntityState.Detached;
                    break;
                    case EntityState.Modified:
                    case EntityState.Deleted:
                    entry.Reload();
                    break;
                }
            }
        }

        public IDbContextTransaction Begin()
        {
            _transaction = _context.Database.BeginTransaction();
            return _transaction;
        }

        public bool Commit()
        {
            bool r = false;

            try
            {
                _context.SaveChanges();
                _transaction.Commit();
                r = true;
            }
            catch
            {
                _transaction.Rollback();
                r = false;
            }
            finally
            {
                _transaction.Dispose();
            }

            return r;
        }

        public async Task<bool> ExecuteStrategy(IEnumerable<Func<Task>> actions)
        {
            var executionStrategy = _context.Database.CreateExecutionStrategy();
            return await executionStrategy.ExecuteAsync(async () =>
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var action in actions)
                        {
                            await action();
                        }

                        await _context.SaveChangesAsync();

                        transaction.Commit();
                        return true;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            });
        }
    }
}
