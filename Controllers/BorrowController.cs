using System.Security.Claims;
using Library.Data;
using Library.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.Controllers
{
    public class BorrowController : Controller
    {
        private readonly LibrarydbContext _dbContext;
        public readonly UserManager<User> _userManager;


        public BorrowController(LibrarydbContext dbContext, UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var books = _dbContext.books.ToList();
            ViewBag.UserId = 1; // 假設從 Session 或認證系統獲取
            return View(books);
        }

        [HttpPost]
        public async Task<IActionResult> Borrow(int bookId)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                var userName = User?.Identity?.Name;
                if (string.IsNullOrEmpty(userName))
                {
                    return RedirectToAction("Login", "Account");
                }

                var book = await _dbContext.books.FirstOrDefaultAsync(b => b.Id == bookId);
                if (book == null)
                {
                    return RedirectToAction("Index");
                }

                if (book.Quantity <= 0)
                {
                    return RedirectToAction("Index");
                }

                var alreadyBorrowed = _dbContext.BorrowRecords
                                                .Any(br => br.BookId == bookId && br.UserName == userName && br.ReturnDate == null);
                if (alreadyBorrowed)
                {
                    return RedirectToAction("Index");
                }

                // 借書
                book.Quantity--;

                var record = new BorrowRecord
                {
                    BookId = bookId,
                    UserName = userName,
                    BorrowDate = DateTime.Now
                };

                _dbContext.BorrowRecords.Add(record);

                await _dbContext.SaveChangesAsync(); // 這裡會檢查 RowVersion
                await transaction.CommitAsync();

                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                await transaction.RollbackAsync();
                TempData["ErrorMessage"] = "借書失敗，該書可能被其他人借走。";
                return RedirectToAction("Index");
            }
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
        public async Task<IActionResult> ConfirmBorrow([FromBody] BorrowRequest request)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                //if (string.IsNullOrEmpty(userId))
                //{
                //    return Json(new { success = false, message = "請先登入！" });
                //}
                var userId = User.Identity?.Name;
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "請先登入！" });
                }

                List<object> borrowedBooks = new();

                foreach (var bookId in request.SelectBooks)
                {
                    var book = await _dbContext.books.FirstOrDefaultAsync(b => b.Id == bookId);
                    if (book == null || book.Quantity <= 0)
                    {
                        return Json(new { success = false, message = $"{book?.Title ?? "此書"} 無庫存，無法借閱。" });
                    }

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

                await _dbContext.SaveChangesAsync(); // 檢查 RowVersion
                await transaction.CommitAsync();

                return Json(new { success = true, message = "📖 借閱成功！", borrowedBooks });
            }
            catch (DbUpdateConcurrencyException)
            {
                await transaction.RollbackAsync();
                return Json(new { success = false, message = "借書失敗，可能已被其他人借走。" });
            }
        }

    }
}
