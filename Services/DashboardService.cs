using Library.Data;
using Library.Repositories.Interfaces;
using Library.Services.Interfaces;
using Library.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Library.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly LibrarydbContext _context;
        private readonly IFineRepository _fineRepo;

        public DashboardService(LibrarydbContext context, IFineRepository fineRepo)
        {
            _context = context;
            _fineRepo = fineRepo;
        }

        public async Task<DashboardViewModel> GetDashboardDataAsync()
        {
            var now = DateTime.Now;
            var monthStart = new DateTime(now.Year, now.Month, 1);

            var activeBorrows = await _context.BorrowRecords.CountAsync(br => br.ReturnDate == null);
            var overdueCount = await _context.BorrowRecords
                .CountAsync(br => br.ReturnDate == null && br.DueDate != null && br.DueDate < now);
            var totalMembers = await _context.Users.CountAsync();
            var totalBooks = await _context.books.CountAsync(b => !b.IsDelete);
            var totalIncome = await _context.incomeRecords
                .Where(ir => ir.PaymentDate >= monthStart)
                .SumAsync(ir => ir.Money);
            var totalExpense = await _context.expenses
                .Where(e => e.ExpenseDate >= monthStart)
                .SumAsync(e => e.Amount);
            var unpaidFines = await _context.fines.CountAsync(f => !f.IsPaid);

            return new DashboardViewModel
            {
                ActiveBorrows = activeBorrows,
                OverdueCount = overdueCount,
                TotalMembers = totalMembers,
                TotalBooks = totalBooks,
                TotalIncome = totalIncome,
                TotalExpense = totalExpense,
                UnpaidFines = unpaidFines
            };
        }
    }
}
