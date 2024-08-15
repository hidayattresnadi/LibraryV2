using Microsoft.EntityFrameworkCore;
using SimpleLibraryV2.Models;

namespace SimpleLibraryV2.Context
{
    public class MyDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Borrowing> Borrowings { get; set; }
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {

        }
    }
}
