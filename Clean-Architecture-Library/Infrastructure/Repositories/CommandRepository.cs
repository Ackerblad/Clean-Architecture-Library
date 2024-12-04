using Application.Interfaces.RepositoryInterfaces;
using Infrastructure.Database;

namespace Infrastructure.Repositories
{
    public class CommandRepository<TEntity> : ICommandRepository<TEntity> where TEntity : class
    {
        private readonly CleanArchitectureLibraryDb _db;

        public CommandRepository(CleanArchitectureLibraryDb db)
        {
            _db = db;
        }

        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            await _db.Set<TEntity>().AddAsync(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            _db.Set<TEntity>().Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _db.Set<TEntity>().FindAsync(id);
            if (entity == null) return false;

            _db.Set<TEntity>().Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
