using Library.Data;
using Library.Models;
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
        public async Task<IActionResult> ReturnBook(int borrowRecordId, string userName)
        {
            var borrowRecord = await _context.BorrowRecords
                .Include(br => br.Book)
                .FirstOrDefaultAsync(br => br.Id == borrowRecordId);

            if (borrowRecord == null)
            {
                return NotFound();
            }

            // 更新歸還時間
            borrowRecord.ReturnDate = DateTime.Now;

            // 更新書籍數量
            var book = borrowRecord.Book;
            if (book != null)
            {
                book.Quantity += 1;
            }

            await _context.SaveChangesAsync();

            // 找出未通知的預約者，不能是自己
            var reservation = await _context.reserveRecords
                .Where(r => r.BookId == borrowRecord.BookId && !r.IsNotify)
                .OrderBy(r => r.ReserveDate)
                .FirstOrDefaultAsync();

            if (reservation != null && reservation.UserName != borrowRecord.UserName)
            {
                var notification = new Notify
                {
                    UserName = reservation.UserName,
                    Message = $"您預約的《{book.Title}》已歸還，可以借閱了！",
                    CreatedAt = DateTime.Now
                };

                _context.notifies.Add(notification);

                reservation.IsNotify = true;

                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", new { userName });
        }       
    }
}
