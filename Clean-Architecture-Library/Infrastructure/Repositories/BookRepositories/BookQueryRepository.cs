using Application.Interfaces.RepositoryInterfaces;
using Domain.Entities;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.BookRepositories
{
    public class BookQueryRepository : IQueryRepository<Book>
    {
        private readonly CleanArchitectureLibraryDb _db;

        public BookQueryRepository(CleanArchitectureLibraryDb db)
        {
            _db = db;
        }

        public async Task<Book> GetByIdAsync(Guid id)
        {
            return await _db.Books.FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            return await _db.Books.ToListAsync();
        }
    }
}
