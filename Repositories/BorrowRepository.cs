using Library.Data;
using Library.Models;
using Library.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Library.Repositories
{
    public class BorrowRepository : IBorrowRepository
    {
        private readonly LibrarydbContext _context;

        public BorrowRepository(LibrarydbContext context)
        {
            _context = context;
        }

        public async Task<BorrowRecord?> GetByIdAsync(int id)
        {
            return await _context.BorrowRecords
                .Include(br => br.Book)
                .FirstOrDefaultAsync(br => br.Id == id);
        }

        public async Task<IEnumerable<BorrowRecord>> GetActiveByUserAsync(string userName)
        {
            return await _context.BorrowRecords
                .Include(br => br.Book)
                .Where(br => br.UserName == userName && br.ReturnDate == null)
                .ToListAsync();
        }

        public async Task<bool> IsAlreadyBorrowedAsync(int bookId, string userName)
        {
            return await _context.BorrowRecords
                .AnyAsync(br => br.BookId == bookId
                    && br.UserName == userName
                    && br.ReturnDate == null);
        }

        public async Task<int> CountActiveByUserAsync(string userName)
        {
            return await _context.BorrowRecords
                .CountAsync(br => br.UserName == userName && br.ReturnDate == null);
        }

        public async Task AddAsync(BorrowRecord record)
        {
            await _context.BorrowRecords.AddAsync(record);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(BorrowRecord record)
        {
            _context.BorrowRecords.Update(record);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<BorrowRecord>> GetAllOverdueAsync() =>
            await _context.BorrowRecords
                .Where(br => br.ReturnDate == null && br.DueDate < DateTime.Now)
                .Include(br => br.Book)
                .ToListAsync();
    }
}
