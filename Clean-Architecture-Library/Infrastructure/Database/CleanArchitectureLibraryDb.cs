using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database
{
    public class CleanArchitectureLibraryDb : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<User> Users { get; set; }

        public CleanArchitectureLibraryDb(DbContextOptions<CleanArchitectureLibraryDb> options) : base(options) { }
    }
}
