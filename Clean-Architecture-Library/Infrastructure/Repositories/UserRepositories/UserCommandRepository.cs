using Application.Interfaces.RepositoryInterfaces;
using Domain.Entities;
using Infrastructure.Database;

namespace Infrastructure.Repositories.UserRepositories
{
    public class UserCommandRepository : ICommandRepository<User>
    {
        private readonly CleanArchitectureLibraryDb _db;

        public UserCommandRepository(CleanArchitectureLibraryDb db)
        {
            _db = db;
        }

        public async Task<User> CreateAsync(User newUser)
        {
            await _db.Users.AddAsync(newUser);
            await _db.SaveChangesAsync();
            return newUser;
        }

        public async Task<User> UpdateAsync(User updatedUser)
        {
            _db.Users.Update(updatedUser);
            await _db.SaveChangesAsync();
            return updatedUser;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var userToDelete = await _db.Users.FindAsync(id);
            if (userToDelete == null) return false;

            _db.Users.Remove(userToDelete);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
