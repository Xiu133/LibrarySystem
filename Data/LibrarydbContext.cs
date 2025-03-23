using Library.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Library.Data
{
    public class LibrarydbContext : IdentityDbContext<IdentityUser>
    {
        public LibrarydbContext(DbContextOptions<LibrarydbContext> options) : base(options)
        {
        }

        public async Task ExecuteTransactionAsync(Func<Task> action)
        {
            using (var transaction = await Database.BeginTransactionAsync())
            {
                try
                {
                    await action(); 
                    await transaction.CommitAsync(); 
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync(); 
                    throw; 
                }
            }
        }

        public DbSet<Book> books { get; set; }
        public DbSet<BorrowRecord> BorrowRecords { get; set; }
        public DbSet<User> users { get; set; }
    }
}
