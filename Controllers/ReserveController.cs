using System.Net;
using Humanizer;
using Library.Data;
using Library.Models;
using Library.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Library.Controllers.BorrowController;

namespace Library.Controllers
{
    public class ReserveController : Controller
    {
        private readonly LibrarydbContext _context;
        private readonly UserManager<User> _userManager;

        public ReserveController(LibrarydbContext context,UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var currentUser = User?.Identity?.Name;

            var borrowedBooks = _context.BorrowRecords
                     .Where(br => br.ReturnDate == null &&
                     br.Book != null &&
                     br.Book.Quantity == 0 &&
                     br.UserName != currentUser)
                    .Include(br => br.Book)
                    .Select(br => br.Book)
                    .Distinct()
                    .ToList();
            return View(borrowedBooks);
        }

        public IActionResult Reserve(int BookId)
        {
            var userName = User?.Identity?.Name;
            if (string.IsNullOrEmpty(userName))
            {
                return RedirectToAction("Login", "Account");
            }

            var book = _context.books.Find(BookId);
            if (book == null)
            {
                ModelState.AddModelError("", "書籍不存在。");
                return RedirectToAction("Index");
            }

            // 判斷書是否「可預約」：目前已借出（沒有任何一本被還）
            bool isAllBorrowed = _context.BorrowRecords
                .Any(br => br.BookId == BookId && br.ReturnDate == null);

            if (!isAllBorrowed)
            {
                ModelState.AddModelError("", "該書目前有庫存，不需預約。");
                return RedirectToAction("Index");
            }

            bool alreadyReserved = _context.reserveRecords
                .Any(r => r.BookId == BookId);

            if (alreadyReserved)
            {
                ModelState.AddModelError("", "該書已被預約。");
                return RedirectToAction("Index");
            }

            // 建立預約
            var reserve = new ReserveRecord
            {
                BookId = BookId,
                UserName = userName,
                ReserveDate = DateTime.Now
            };

            _context.reserveRecords.Add(reserve);
            _context.SaveChanges();

            TempData["Message"] = $"成功預約：{book.Title}";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ConfirmReserve([FromBody] BorrowRequest request)
        {
            if (request?.SelectBooks == null || !request.SelectBooks.Any())
            {
                return Json(new { success = false, message = "請選擇要預約的書籍。" });
            }

            var userId = User.Identity?.Name;
            if (string.IsNullOrEmpty(userId))
            {
                return Json(new { success = false, message = "請先登入！" });
            }

            List<object> reservedBooks = new();

            using var transaction = _context.Database.BeginTransaction(); // 🔑 開始交易
            try
            {
                foreach (var bookId in request.SelectBooks)
                {
                    var book = _context.books
                        .Where(b => b.Id == bookId)
                        .FirstOrDefault();

                    if (book == null) continue;

                    // 必須已借光
                    bool isAllBorrowed = _context.BorrowRecords.Any(br => br.BookId == bookId && br.ReturnDate == null);
                    bool alreadyReserved = _context.reserveRecords.Any(r => r.BookId == bookId && r.IsNotify == false);

                    if (isAllBorrowed && !alreadyReserved)
                    {
                        var reserve = new ReserveRecord
                        {
                            Title = book.Title,
                            BookId = bookId,
                            UserName = userId,
                            ReserveDate = DateTime.Now
                        };

                        _context.reserveRecords.Add(reserve);

                        reservedBooks.Add(new
                        {
                            Title = book.Title,
                            ReserveDate = reserve.ReserveDate.ToString("yyyy-MM-dd"),
                            Status = "預約成功"
                        });
                    }
                    else if (!isAllBorrowed)
                    {
                        return Json(new { success = false, message = $"{book.Title} 尚有庫存，請直接借閱。" });
                    }
                    else
                    {
                        return Json(new { success = false, message = $"{book.Title} 已被預約，無法重複預約。" });
                    }
                }

                _context.SaveChanges(); // 🔑 嘗試提交
                transaction.Commit();   // 🔑 完成交易
            }
            catch (DbUpdateConcurrencyException)
            {
                transaction.Rollback(); // 回滾
                return Json(new { success = false, message = "⚠️ 系統偵測到同時預約衝突，請重新嘗試。" });
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return Json(new { success = false, message = $"❌ 發生錯誤: {ex.Message}" });
            }

            return Json(new { success = true, message = "📘 預約完成！", reservedBooks });
        }


    }
}
