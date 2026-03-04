using Library.Models;
using Library.ViewModels;

namespace Library.Services.Interfaces
{
    public interface IReserveService
    {
        Task<IEnumerable<Book>> GetReservableBooksAsync(string currentUser);
        Task<ServiceResult> ReserveBookAsync(int bookId, string userName);
        Task<ServiceResult> ReserveBooksAsync(IEnumerable<int> bookIds, string userName);
    }
}
