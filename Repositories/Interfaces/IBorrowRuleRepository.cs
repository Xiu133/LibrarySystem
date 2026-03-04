using Library.Models;

namespace Library.Repositories.Interfaces
{
    public interface IBorrowRuleRepository
    {
        Task<BorrowRule?> GetByRoleAsync(string role);
        Task<IEnumerable<BorrowRule>> GetAllAsync();
        Task AddAsync(BorrowRule rule);
        Task<bool> UpdateAsync(BorrowRule rule);
        Task<IEnumerable<string>> GetDistinctRolesAsync();
    }
}
