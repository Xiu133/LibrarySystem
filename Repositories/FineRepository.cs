using Library.Data;
using Library.Models;
using Library.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Library.Repositories
{
    public class FineRepository : IFineRepository
    {
        private readonly LibrarydbContext _context;
        public FineRepository(LibrarydbContext context) => _context = context;

        public async Task<IEnumerable<Fine>> GetUnpaidAsync() =>
            await _context.fines.Include(f => f.BorrowRecord).ThenInclude(br => br!.Book)
                .Where(f => !f.IsPaid).OrderByDescending(f => f.CreatedAt).ToListAsync();

        public async Task<IEnumerable<Fine>> GetByUserAsync(string userName) =>
            await _context.fines.Where(f => f.UserName == userName)
                .OrderByDescending(f => f.CreatedAt).ToListAsync();

        public async Task<IEnumerable<Fine>> GetAllAsync() =>
            await _context.fines.Include(f => f.BorrowRecord).ThenInclude(br => br!.Book)
                .OrderByDescending(f => f.CreatedAt).ToListAsync();

        public async Task<Fine?> GetByBorrowRecordAsync(int borrowRecordId) =>
            await _context.fines.FirstOrDefaultAsync(f => f.BorrowRecordId == borrowRecordId);

        public async Task AddAsync(Fine fine)
        {
            await _context.fines.AddAsync(fine);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(Fine fine)
        {
            _context.fines.Update(fine);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
