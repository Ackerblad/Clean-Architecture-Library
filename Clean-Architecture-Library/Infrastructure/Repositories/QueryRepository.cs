using Application.Interfaces.RepositoryInterfaces;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class QueryRepository<TEntity> : IQueryRepository<TEntity> where TEntity : class
    {
        private readonly CleanArchitectureLibraryDb _db;

        public QueryRepository(CleanArchitectureLibraryDb db)
        {
            _db = db;
        }

        public async Task<TEntity?> GetByIdAsync(Guid id)
        {
            return await _db.Set<TEntity>().FindAsync(id);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _db.Set<TEntity>().ToListAsync();
        }
    }
}
