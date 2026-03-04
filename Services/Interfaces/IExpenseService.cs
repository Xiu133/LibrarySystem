using Library.Models;
using Library.ViewModels;

namespace Library.Services.Interfaces
{
    public interface IExpenseService
    {
        Task<IEnumerable<Expense>> GetAllExpensesAsync();
        Task<IEnumerable<Expense>> GetByMonthAsync(int year, int month);
        Task<ServiceResult> AddExpenseAsync(string category, decimal amount, string? description, DateTime date, string createdBy);
        Task<ServiceResult> DeleteExpenseAsync(int id);
        Task<decimal> GetTotalExpenseAsync(DateTime from, DateTime to);
    }
}
