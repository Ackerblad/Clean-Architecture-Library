using Application.Interfaces.RepositoryInterfaces;
using Domain.Entities;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.AuthorRepositories
{
    public class AuthorQueryRepository : IQueryRepository<Author>
    {
        private readonly CleanArchitectureLibraryDb _db;

        public AuthorQueryRepository(CleanArchitectureLibraryDb db)
        {
            _db = db;
        }

        public async Task<Author> GetByIdAsync(Guid id)
        {
            return await _db.Authors.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Author>> GetAllAsync()
        {
            return await _db.Authors.ToListAsync();
        }
    }
}
