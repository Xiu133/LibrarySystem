using Library.Models;
using Library.Repositories.Interfaces;
using Library.Services.Interfaces;

namespace Library.Services
{
    public class NotifyService : INotifyService
    {
        private readonly INotifyRepository _notifyRepository;
        private readonly IReserveRepository _reserveRepository;

        public NotifyService(INotifyRepository notifyRepository, IReserveRepository reserveRepository)
        {
            _notifyRepository = notifyRepository;
            _reserveRepository = reserveRepository;
        }

        public async Task<IEnumerable<Notify>> GetNotificationsForUserAsync(string userName)
        {
            return await _notifyRepository.GetByUserAsync(userName);
        }

        public async Task CreateNotificationAsync(string userName, string message)
        {
            var notification = new Notify
            {
                UserName = userName,
                Message = message,
                CreatedAt = DateTime.Now
            };
            await _notifyRepository.AddAsync(notification);
        }

        public async Task NotifyFirstReservationAsync(int bookId, string returnedByUser, string? bookTitle)
        {
            var reservation = await _reserveRepository.GetFirstUnnotifiedAsync(bookId);

            if (reservation == null || reservation.UserName == returnedByUser)
                return;

            var notification = new Notify
            {
                UserName = reservation.UserName,
                Message = $"您預約的《{bookTitle}》已歸還，可以借閱了！",
                CreatedAt = DateTime.Now
            };

            await _notifyRepository.AddAsync(notification);

            reservation.IsNotify = true;
            await _reserveRepository.UpdateAsync(reservation);
        }
    }
}
