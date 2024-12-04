using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database
{
    public class CleanArchitectureLibrary : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<User> Users { get; set; }

        public CleanArchitectureLibrary(DbContextOptions<CleanArchitectureLibrary> options) : base(options) { }
    }
}
