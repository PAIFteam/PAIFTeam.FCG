using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PAIFGAMES.FCG.Domain.Data;
using PAIFGAMES.FCG.Infra.Context;

namespace PAIFGAMES.FCG.Infra.Data
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly ApplicationDbContext _context;
        protected DbSet<TEntity> DbSet;
        protected readonly ILogger _logger;
        public IQueryable<TEntity> Entities => DbSet;

        public GenericRepository(ApplicationDbContext context, ILogger logger)
        {
            _context = context;
            DbSet = _context.Set<TEntity>();
            _logger = logger;
        }
    }
}