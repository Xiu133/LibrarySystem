using Library.Models;
using Library.Repositories.Interfaces;
using Library.Services.Interfaces;
using Library.ViewModels;

namespace Library.Services
{
    public class ReserveService : IReserveService
    {
        private readonly IReserveRepository _reserveRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IBorrowRepository _borrowRepository;

        public ReserveService(
            IReserveRepository reserveRepository,
            IBookRepository bookRepository,
            IBorrowRepository borrowRepository)
        {
            _reserveRepository = reserveRepository;
            _bookRepository = bookRepository;
            _borrowRepository = borrowRepository;
        }

        public async Task<IEnumerable<Book>> GetReservableBooksAsync(string currentUser)
        {
            var allBooks = await _bookRepository.GetAllAsync();
            var result = new List<Book>();

            foreach (var book in allBooks)
            {
                if (book.Quantity == 0 &&
                    await _borrowRepository.IsAlreadyBorrowedAsync(book.Id, currentUser) == false)
                {
                    result.Add(book);
                }
            }

            return result;
        }

        public async Task<ServiceResult> ReserveBookAsync(int bookId, string userName)
        {
            var book = await _bookRepository.GetByIdAsync(bookId);
            if (book == null)
                return ServiceResult.Fail("書籍不存在。");

            bool isAllBorrowed = await _borrowRepository.IsAlreadyBorrowedAsync(bookId, userName);
            if (!isAllBorrowed)
                return ServiceResult.Fail("該書目前有庫存，不需預約。");

            if (await _reserveRepository.IsAlreadyReservedAsync(bookId))
                return ServiceResult.Fail("該書已被預約。");

            var reserve = new ReserveRecord
            {
                BookId = bookId,
                Title = book.Title,
                UserName = userName,
                ReserveDate = DateTime.Now
            };

            await _reserveRepository.AddAsync(reserve);
            return ServiceResult.Ok(new { Title = book.Title });
        }

        public async Task<ServiceResult> ReserveBooksAsync(IEnumerable<int> bookIds, string userName)
        {
            var reservedBooks = new List<object>();

            foreach (var bookId in bookIds)
            {
                var book = await _bookRepository.GetByIdAsync(bookId);
                if (book == null) continue;

                bool isAllBorrowed = await _borrowRepository.IsAlreadyBorrowedAsync(bookId, userName);
                bool alreadyReserved = await _reserveRepository.IsAlreadyReservedAsync(bookId);

                if (!isAllBorrowed)
                    return ServiceResult.Fail($"{book.Title} 尚有庫存，請直接借閱。");

                if (alreadyReserved)
                    return ServiceResult.Fail($"{book.Title} 已被預約，無法重複預約。");

                var reserve = new ReserveRecord
                {
                    Title = book.Title,
                    BookId = bookId,
                    UserName = userName,
                    ReserveDate = DateTime.Now
                };

                await _reserveRepository.AddAsync(reserve);
                reservedBooks.Add(new
                {
                    Title = book.Title,
                    ReserveDate = reserve.ReserveDate.ToString("yyyy-MM-dd"),
                    Status = "預約成功"
                });
            }

            return ServiceResult.Ok(reservedBooks);
        }
    }
}
