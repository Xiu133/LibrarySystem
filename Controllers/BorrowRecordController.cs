using Library.Data;
using Library.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.Controllers
{
    public class BorrowRecordController : Controller
    {
        private readonly LibrarydbContext _dbContext;

        public BorrowRecordController(LibrarydbContext librarydbContext)
        {
            _dbContext = librarydbContext;
        }

        public IActionResult Index()
        {
            var userId = User.Identity?.Name;

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var borrowRecords = _dbContext.BorrowRecords
                                          .Where(br => br.UserName == userId)
                                          .ToList();

            var books = _dbContext.books.ToList();

            var borrowRecordWithBooks = borrowRecords.Select(br => new
            {
                BorrowRecord = br,
                BookTitle = books.FirstOrDefault(b => b.Id == br.BookId)?.Title,
                ReturnDate = br.ReturnDate
            }).ToList();

            return View(borrowRecordWithBooks);
        }
    }
}
