using Library.Data;
using Library.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.Controllers.Finance
{
    [Authorize(Roles = "Admin")]
    public class FinanceTotalController : Controller
    {
        private readonly LibrarydbContext _context;
        private readonly IExpenseService _expenseService;

        public FinanceTotalController(LibrarydbContext context, IExpenseService expenseService)
        {
            _context = context;
            _expenseService = expenseService;
        }

        public IActionResult Index() => View();

        [HttpGet]
        public async Task<IActionResult> GetIncomeData()
        {
            var data = await _context.incomeRecords
                .GroupBy(i => i.Reason)
                .Select(g => new { Reason = g.Key, Total = g.Sum(x => x.Money) })
                .ToListAsync();
            return Json(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetExpenseData()
        {
            var from = DateTime.Now.AddMonths(-11);
            var to = DateTime.Now;
            var data = await _context.expenses
                .Where(e => e.ExpenseDate >= from && e.ExpenseDate <= to)
                .GroupBy(e => new { e.ExpenseDate.Year, e.ExpenseDate.Month })
                .Select(g => new { Month = g.Key.Year + "/" + g.Key.Month, Total = g.Sum(e => e.Amount) })
                .OrderBy(x => x.Month)
                .ToListAsync();
            return Json(data);
        }
    }
}
