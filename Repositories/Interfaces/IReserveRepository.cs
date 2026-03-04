using Library.Models;

namespace Library.Repositories.Interfaces
{
    public interface IReserveRepository
    {
        Task<bool> IsAlreadyReservedAsync(int bookId);
        Task<ReserveRecord?> GetFirstUnnotifiedAsync(int bookId);
        Task AddAsync(ReserveRecord record);
        Task<bool> UpdateAsync(ReserveRecord record);
    }
}
