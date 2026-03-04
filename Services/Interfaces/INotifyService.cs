using Library.Models;

namespace Library.Services.Interfaces
{
    public interface INotifyService
    {
        Task<IEnumerable<Notify>> GetNotificationsForUserAsync(string userName);
        Task CreateNotificationAsync(string userName, string message);
        Task NotifyFirstReservationAsync(int bookId, string returnedByUser, string? bookTitle);
    }
}
