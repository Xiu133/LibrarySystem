using Library.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers.Finance
{
    [Authorize(Roles = "Admin")]
    public class ExpenseController : Controller
    {
        private readonly IExpenseService _expenseService;
        public ExpenseController(IExpenseService expenseService) => _expenseService = expenseService;

        public async Task<IActionResult> Index()
        {
            var expenses = await _expenseService.GetAllExpensesAsync();
            return View(expenses);
        }

        [HttpPost]
        public async Task<IActionResult> Create(string category, decimal amount, string? description)
        {
            var userName = User.Identity?.Name ?? "Admin";
            await _expenseService.AddExpenseAsync(category, amount, description, DateTime.Now, userName);
            TempData["Success"] = "費用記錄已新增！";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _expenseService.DeleteExpenseAsync(id);
            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            var expenses = await _expenseService.GetAllExpensesAsync();
            return Json(expenses.Select(e => new
            {
                e.Id,
                e.Category,
                e.Amount,
                e.Description,
                ExpenseDate = e.ExpenseDate.ToString("yyyy-MM-dd"),
                e.CreatedBy
            }));
        }
    }
}
