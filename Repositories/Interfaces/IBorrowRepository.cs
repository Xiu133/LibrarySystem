using Library.Models;

namespace Library.Repositories.Interfaces
{
    public interface IBorrowRepository
    {
        Task<BorrowRecord?> GetByIdAsync(int id);
        Task<IEnumerable<BorrowRecord>> GetActiveByUserAsync(string userName);
        Task<bool> IsAlreadyBorrowedAsync(int bookId, string userName);
        Task<int> CountActiveByUserAsync(string userName);
        Task AddAsync(BorrowRecord record);
        Task<bool> UpdateAsync(BorrowRecord record);
        Task<IEnumerable<BorrowRecord>> GetAllOverdueAsync();
    }
}
