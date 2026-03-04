using Library.Models;
namespace Library.Repositories.Interfaces
{
    public interface IExpenseRepository
    {
        Task<IEnumerable<Expense>> GetAllAsync();
        Task<IEnumerable<Expense>> GetByMonthAsync(int year, int month);
        Task AddAsync(Expense expense);
        Task DeleteAsync(int id);
        Task<decimal> GetTotalAsync(DateTime from, DateTime to);
    }
}
