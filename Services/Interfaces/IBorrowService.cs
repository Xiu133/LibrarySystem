using Library.Models;
using Library.ViewModels;

namespace Library.Services.Interfaces
{
    public interface IBorrowService
    {
        Task<ServiceResult> BorrowBookAsync(int bookId, string userName);
        Task<ServiceResult> BorrowBooksAsync(IEnumerable<int> bookIds, string userName);
        Task<ServiceResult> ReturnBookAsync(int borrowRecordId);
        Task<IEnumerable<BorrowRecord>> GetActiveRecordsByUserAsync(string userName);
    }
}
