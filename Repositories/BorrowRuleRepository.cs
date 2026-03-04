using Library.Data;
using Library.Models;
using Library.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Library.Repositories
{
    public class BorrowRuleRepository : IBorrowRuleRepository
    {
        private readonly LibrarydbContext _context;

        public BorrowRuleRepository(LibrarydbContext context)
        {
            _context = context;
        }

        public async Task<BorrowRule?> GetByRoleAsync(string role)
        {
            return await _context.BorrowRule.FirstOrDefaultAsync(r => r.Role == role);
        }

        public async Task<IEnumerable<BorrowRule>> GetAllAsync()
        {
            return await _context.BorrowRule.ToListAsync();
        }

        public async Task AddAsync(BorrowRule rule)
        {
            await _context.BorrowRule.AddAsync(rule);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(BorrowRule rule)
        {
            _context.BorrowRule.Update(rule);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<string>> GetDistinctRolesAsync()
        {
            return await _context.Users
                .Select(u => u.Role)
                .Distinct()
                .Where(r => !string.IsNullOrEmpty(r))
                .Select(r => r!)
                .ToListAsync();
        }
    }
}
