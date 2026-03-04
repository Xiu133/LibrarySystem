using Library.Models;
namespace Library.Repositories.Interfaces
{
    public interface IFineRepository
    {
        Task<IEnumerable<Fine>> GetUnpaidAsync();
        Task<IEnumerable<Fine>> GetByUserAsync(string userName);
        Task<IEnumerable<Fine>> GetAllAsync();
        Task<Fine?> GetByBorrowRecordAsync(int borrowRecordId);
        Task AddAsync(Fine fine);
        Task<bool> UpdateAsync(Fine fine);
    }
}
