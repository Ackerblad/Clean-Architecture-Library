using Application.Interfaces.RepositoryInterfaces;
using Domain.Entities;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.UserRepositories
{
    public class UserQueryRepository : IQueryRepository<User>
    {
        private readonly CleanArchitectureLibraryDb _db;

        public UserQueryRepository(CleanArchitectureLibraryDb db)
        {
            _db = db;
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _db.Users.ToListAsync();
        }
    }
}
