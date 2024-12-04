using Application.Interfaces.RepositoryInterfaces;
using Domain.Entities;
using Infrastructure.Database;

namespace Infrastructure.Repositories.BookRepositories
{
    public class BookCommandRepository : ICommandRepository<Book>
    {
        private readonly CleanArchitectureLibraryDb _db;

        public BookCommandRepository(CleanArchitectureLibraryDb db)
        {
            _db = db;
        }

        public async Task<Book> CreateAsync(Book newBook)
        {
            await _db.Books.AddAsync(newBook);
            await _db.SaveChangesAsync();
            return newBook;
        }

        public async Task<Book> UpdateAsync(Book updatedBook)
        {
            _db.Books.Update(updatedBook);
            await _db.SaveChangesAsync();
            return updatedBook;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var bookToDelete = await _db.Books.FindAsync(id);
            if (bookToDelete == null) return false;

            _db.Books.Remove(bookToDelete);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
