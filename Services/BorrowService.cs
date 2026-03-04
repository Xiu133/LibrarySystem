using Library.Models;
using Library.Repositories.Interfaces;
using Library.Services.Interfaces;
using Library.ViewModels;

namespace Library.Services
{
    public class BorrowService : IBorrowService
    {
        private readonly IBorrowRepository _borrowRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IBorrowRuleService _borrowRuleService;
        private readonly INotifyService _notifyService;

        public BorrowService(
            IBorrowRepository borrowRepository,
            IBookRepository bookRepository,
            IBorrowRuleService borrowRuleService,
            INotifyService notifyService)
        {
            _borrowRepository = borrowRepository;
            _bookRepository = bookRepository;
            _borrowRuleService = borrowRuleService;
            _notifyService = notifyService;
        }

        public async Task<ServiceResult> BorrowBookAsync(int bookId, string userName)
        {
            var book = await _bookRepository.GetByIdAsync(bookId);
            if (book == null)
                return ServiceResult.Fail("書籍不存在。");

            if (book.Quantity <= 0)
                return ServiceResult.Fail("該書目前無庫存。");

            if (await _borrowRepository.IsAlreadyBorrowedAsync(bookId, userName))
                return ServiceResult.Fail("您已借閱此書。");

            var activeCount = await _borrowRepository.CountActiveByUserAsync(userName);
            var maxBooks = await _borrowRuleService.GetMaxBooksForRoleAsync("User");
            if (activeCount >= maxBooks)
                return ServiceResult.Fail($"已達借書上限（{maxBooks} 本）。");

            book.Quantity--;
            await _bookRepository.UpdateAsync(book);

            var record = new BorrowRecord
            {
                BookId = bookId,
                UserName = userName,
                BorrowDate = DateTime.Now
            };
            await _borrowRepository.AddAsync(record);

            return ServiceResult.Ok();
        }

        public async Task<ServiceResult> BorrowBooksAsync(IEnumerable<int> bookIds, string userName)
        {
            var idList = bookIds.ToList();
            var activeCount = await _borrowRepository.CountActiveByUserAsync(userName);
            var maxBooks = await _borrowRuleService.GetMaxBooksForRoleAsync("User");

            if (activeCount + idList.Count > maxBooks)
                return ServiceResult.Fail($"超過借書上限（{maxBooks} 本）。");

            var borrowedBooks = new List<object>();

            foreach (var bookId in idList)
            {
                var book = await _bookRepository.GetByIdAsync(bookId);
                if (book == null || book.Quantity <= 0)
                    return ServiceResult.Fail($"{book?.Title ?? "此書"} 無庫存，無法借閱。");

                book.Quantity--;
                await _bookRepository.UpdateAsync(book);

                var record = new BorrowRecord
                {
                    BookId = bookId,
                    UserName = userName,
                    BorrowDate = DateTime.Now
                };
                await _borrowRepository.AddAsync(record);

                borrowedBooks.Add(new
                {
                    Title = book.Title,
                    BorrowDate = record.BorrowDate.ToString("yyyy-MM-dd"),
                    Status = "借閱中"
                });
            }

            return ServiceResult.Ok(borrowedBooks);
        }

        public async Task<ServiceResult> ReturnBookAsync(int borrowRecordId)
        {
            var record = await _borrowRepository.GetByIdAsync(borrowRecordId);
            if (record == null)
                return ServiceResult.Fail("借閱記錄不存在。");

            record.ReturnDate = DateTime.Now;
            await _borrowRepository.UpdateAsync(record);

            var book = record.Book;
            if (book != null)
            {
                book.Quantity++;
                await _bookRepository.UpdateAsync(book);
                await _notifyService.NotifyFirstReservationAsync(book.Id, record.UserName, book.Title);
            }

            return ServiceResult.Ok();
        }

        public async Task<IEnumerable<BorrowRecord>> GetActiveRecordsByUserAsync(string userName)
        {
            return await _borrowRepository.GetActiveByUserAsync(userName);
        }
    }
}
