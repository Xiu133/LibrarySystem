using Library.Data;
using Library.Models;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    public class BorrowController : Controller
    {
        private readonly LibrarydbContext _dbContext;

        public BorrowController(LibrarydbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var books = _dbContext.books.ToList();
            ViewBag.UserId = 1; // 假設從 Session 或認證系統獲取
            return View(books);
        }

        [HttpPost]
        public IActionResult Borrow(int bookId)
        {
            var userName = User?.Identity?.Name;
            if (string.IsNullOrEmpty(userName))
            {
                return RedirectToAction("Login", "Account");
            }

            var book = _dbContext.books.Find(bookId);
            if (book == null)
            {
                ModelState.AddModelError("", "書籍不存在。");
                return RedirectToAction("Index");
            }

            var alreadyBorrowed = _dbContext.BorrowRecords
                                             .Any(br => br.BookId == bookId && br.UserName == userName && br.ReturnDate == null);
            if (alreadyBorrowed)
            {
                ModelState.AddModelError("", "您已經借過此書籍，尚未歸還。");
                return RedirectToAction("Index");
            }

            if (book.Quantity <= 0)
            {
                ModelState.AddModelError("", $"書籍 {bookId} 數量不足，無法借閱。");
                return RedirectToAction("Index");
            }

            

            var record = new BorrowRecord
            {
                BookId = bookId,
                UserName = userName,
                BorrowDate = DateTime.Now,
                ReturnDate = null
            };

            _dbContext.BorrowRecords.Add(record);
            book.Quantity--;
            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }



        [HttpPost]
        public IActionResult Return(int recordId)//還書
        {
            var record = _dbContext.BorrowRecords.Find(recordId);
            if (record != null && record.ReturnDate == null)
            {
                record.ReturnDate = DateTime.Now;

                var book = _dbContext.books.Find(record.BookId);
                if (book != null)
                {
                    book.Quantity++;
                }

                _dbContext.SaveChanges();
            }

            return RedirectToAction("BorrowHistory", "Users");
        }

        public class BorrowRequest
        {
            public List<int>? SelectBooks { get; set; }
        }

        [HttpPost]
        public IActionResult ConfirmBorrow([FromBody] BorrowRequest request)
        {
            if (request == null || request.SelectBooks == null || !request.SelectBooks.Any())
            {
                return Json(new { success = false, message = "請選擇要借閱的書籍。" });
            }

            var userId = User.Identity?.Name;
            if (string.IsNullOrEmpty(userId))
            {
                return Json(new { success = false, message = "請先登入！" });
            }

            List<object> borrowedBooks = new List<object>();

            foreach (var bookId in request.SelectBooks)
            {
                var book = _dbContext.books.Find(bookId);
                if (book != null && book.Quantity > 0)
                {
                    book.Quantity--;

                    var borrowRecord = new BorrowRecord
                    {
                        UserName = userId,
                        BookId = bookId,
                        BorrowDate = DateTime.Now
                    };

                    _dbContext.BorrowRecords.Add(borrowRecord);

                    borrowedBooks.Add(new
                    {
                        Title = book.Title,
                        BorrowDate = borrowRecord.BorrowDate.ToString("yyyy-MM-dd"),
                        Status = "借閱中"
                    });
                }
                else
                {
                    return Json(new { success = false, message = $"{book?.Title} 無庫存，無法借閱。" });
                }
            }

            _dbContext.SaveChanges();

            return Json(new { success = true, message = "📖 借閱成功！", borrowedBooks });
        }
    }
}
