using Library.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.Controllers
{
    public class AccountRecordController : Controller
    {
        private readonly LibrarydbContext _context;

        public AccountRecordController(LibrarydbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string? userName)
        {
            if (string.IsNullOrEmpty(userName))  return View(new List<object>());

            var userExists = _context.Users.Any(u => u.UserName == userName);
            if (!userExists)
            {
                ViewBag.ErrorMessage = "此帳號不存在，請重新輸入。";
                return View(new List<object>());
            }

            var borrowRecords = _context.BorrowRecords
                                          .Where(br => br.UserName == userName)
                                          .ToList();
            var books = _context.books.ToList();

            var borrowRecordWithBooks = borrowRecords.Select(br => new
            {
                BorrowRecord = br,
                BookTitle = books.FirstOrDefault(b => b.Id == br.BookId)?.Title
            }).ToList();

            return View(borrowRecordWithBooks);
        }
    }
}
