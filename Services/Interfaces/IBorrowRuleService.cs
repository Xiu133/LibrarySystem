using Library.Models;

namespace Library.Services.Interfaces
{
    public interface IBorrowRuleService
    {
        Task<int> GetMaxBooksForRoleAsync(string role);
        Task SaveBorrowRuleAsync(string role, int maxBooks);
        Task<BorrowRule?> GetRuleByRoleAsync(string role);
        Task<IEnumerable<string>> GetDistinctRolesAsync();
    }
}
