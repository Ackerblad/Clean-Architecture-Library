using Application.Interfaces.RepositoryInterfaces;
using Domain.Entities;
using Infrastructure.Database;

namespace Infrastructure.Repositories.AuthorRepositories
{
    public class AuthorCommandRepository : ICommandRepository<Author>
    {
        private readonly CleanArchitectureLibraryDb _db;

        public AuthorCommandRepository(CleanArchitectureLibraryDb db)
        {
            _db = db;
        }

        public async Task<Author> CreateAsync(Author newAuthor)
        {
            await _db.Authors.AddAsync(newAuthor);
            await _db.SaveChangesAsync();
            return newAuthor;
        }

        public async Task<Author> UpdateAsync(Author updatedAuthor)
        {
            _db.Authors.Update(updatedAuthor);
            await _db.SaveChangesAsync();
            return updatedAuthor;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var authorToDelete = await _db.Authors.FindAsync(id);
            if (authorToDelete == null) return false;

            _db.Authors.Remove(authorToDelete);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
