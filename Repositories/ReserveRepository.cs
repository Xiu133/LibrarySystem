using Library.Data;
using Library.Models;
using Library.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Library.Repositories
{
    public class ReserveRepository : IReserveRepository
    {
        private readonly LibrarydbContext _context;

        public ReserveRepository(LibrarydbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsAlreadyReservedAsync(int bookId)
        {
            return await _context.reserveRecords
                .AnyAsync(r => r.BookId == bookId && !r.IsNotify);
        }

        public async Task<ReserveRecord?> GetFirstUnnotifiedAsync(int bookId)
        {
            return await _context.reserveRecords
                .Where(r => r.BookId == bookId && !r.IsNotify)
                .OrderBy(r => r.ReserveDate)
                .FirstOrDefaultAsync();
        }

        public async Task AddAsync(ReserveRecord record)
        {
            await _context.reserveRecords.AddAsync(record);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(ReserveRecord record)
        {
            _context.reserveRecords.Update(record);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
