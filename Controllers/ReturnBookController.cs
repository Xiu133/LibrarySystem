using Library.Data;
using Library.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.Controllers
{
    public class ReturnBookController : Controller
    {
        private readonly LibrarydbContext _context;

        public ReturnBookController(LibrarydbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string? userName)
        {
            if (string.IsNullOrEmpty(userName)) return View(new List<BorrowRecordViewModel>());

            var userExists = _context.Users.Any(u => u.UserName == userName);//查該UserName是否存在於aspnetusers
            if (!userExists)
            {
                ViewBag.ErrorMessage = "此帳號不存在，請重新輸入。";
                return View(new List<BorrowRecordViewModel>());
            }
            //查還沒歸還的書
            var borrowRecords = _context.BorrowRecords
                                      .Where(br => br.UserName == userName && br.ReturnDate == null)
                                      .ToList();
            var books = _context.books.ToList();

            var borrowRecordWithBooks = borrowRecords.Select(br => new BorrowRecordViewModel
            {
                BorrowRecordId = br.Id,
                Title = books.FirstOrDefault(b => b.Id == br.BookId)?.Title ?? "未知",
                Author = books.FirstOrDefault(b => b.Id == br.BookId)?.Author ?? "未知",
                BorrowDate = br.BorrowDate.ToString("yyyy-MM-dd")
            }).ToList();

            ViewBag.UserName = userName;
            return View(borrowRecordWithBooks);
        }

        [HttpPost]
        public IActionResult ReturnBook(int borrowRecordId, string userName)
        {
            var borrowRecord = _context.BorrowRecords.FirstOrDefault(br => br.Id == borrowRecordId);
            if (borrowRecord == null)
            {
                return NotFound();
            }

            borrowRecord.ReturnDate = DateTime.Now;

            var book = _context.books.FirstOrDefault(b => b.Id == borrowRecord.BookId);
            if (book != null)
            {
                book.Quantity += 1; // 增加庫存數量
            }

            _context.SaveChanges();

            return RedirectToAction("Index", new { userName }); // 重新查詢此帳號的借閱紀錄
        }
    }
}
