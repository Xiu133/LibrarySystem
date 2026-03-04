using Library.Models;
using Library.Repositories.Interfaces;
using Library.Services.Interfaces;
using Library.ViewModels;

namespace Library.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository _repo;
        public ExpenseService(IExpenseRepository repo) => _repo = repo;

        public async Task<IEnumerable<Expense>> GetAllExpensesAsync() => await _repo.GetAllAsync();

        public async Task<IEnumerable<Expense>> GetByMonthAsync(int year, int month) =>
            await _repo.GetByMonthAsync(year, month);

        public async Task<ServiceResult> AddExpenseAsync(string category, decimal amount, string? description, DateTime date, string createdBy)
        {
            if (string.IsNullOrWhiteSpace(category))
                return ServiceResult.Fail("費用類別不可為空。");
            await _repo.AddAsync(new Expense
            {
                Category = category,
                Amount = amount,
                Description = description,
                ExpenseDate = date,
                CreatedBy = createdBy
            });
            return ServiceResult.Ok();
        }

        public async Task<ServiceResult> DeleteExpenseAsync(int id)
        {
            await _repo.DeleteAsync(id);
            return ServiceResult.Ok();
        }

        public async Task<decimal> GetTotalExpenseAsync(DateTime from, DateTime to) =>
            await _repo.GetTotalAsync(from, to);
    }
}
