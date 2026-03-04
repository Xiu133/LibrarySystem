using Library.Data;
using Library.Models;
using Library.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Library.Repositories
{
    public class NotifyRepository : INotifyRepository
    {
        private readonly LibrarydbContext _context;

        public NotifyRepository(LibrarydbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Notify>> GetByUserAsync(string userName)
        {
            return await _context.notifies
                .Where(n => n.UserName == userName)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task AddAsync(Notify notify)
        {
            await _context.notifies.AddAsync(notify);
            await _context.SaveChangesAsync();
        }
    }
}
