using Library.Data;
using Library.Models;
using Library.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Library.Repositories
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly LibrarydbContext _context;
        public ExpenseRepository(LibrarydbContext context) => _context = context;

        public async Task<IEnumerable<Expense>> GetAllAsync() =>
            await _context.expenses.OrderByDescending(e => e.ExpenseDate).ToListAsync();

        public async Task<IEnumerable<Expense>> GetByMonthAsync(int year, int month) =>
            await _context.expenses
                .Where(e => e.ExpenseDate.Year == year && e.ExpenseDate.Month == month)
                .ToListAsync();

        public async Task AddAsync(Expense expense)
        {
            await _context.expenses.AddAsync(expense);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var expense = await _context.expenses.FindAsync(id);
            if (expense != null)
            {
                _context.expenses.Remove(expense);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<decimal> GetTotalAsync(DateTime from, DateTime to) =>
            await _context.expenses
                .Where(e => e.ExpenseDate >= from && e.ExpenseDate <= to)
                .SumAsync(e => e.Amount);
    }
}
