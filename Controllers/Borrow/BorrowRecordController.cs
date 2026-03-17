using Library.Data;
using Library.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.Controllers.Borrow
{
    public class BorrowRecordController : Controller
    {
        private readonly LibrarydbContext _dbContext;

        public BorrowRecordController(LibrarydbContext librarydbContext)
        {
            _dbContext = librarydbContext;
        }

        public IActionResult Index(int page = 1)
        {
            const int pageSize = 10;
            var userId = User.Identity?.Name;

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var query = _dbContext.BorrowRecords
                                  .Where(br => br.UserName == userId)
                                  .OrderByDescending(br => br.BorrowDate);

            var totalCount = query.Count();

            var borrowRecords = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var bookIds = borrowRecords.Select(br => br.BookId).ToList();
            var books = _dbContext.books.Where(b => bookIds.Contains(b.Id)).ToList();

            var borrowRecordWithBooks = borrowRecords.Select(br => new
            {
                BorrowRecord = br,
                BookTitle = books.FirstOrDefault(b => b.Id == br.BookId)?.Title,
                ReturnDate = br.ReturnDate
            }).ToList();

            ViewBag.Page = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            ViewBag.PaginationAction = "Index";

            return View(borrowRecordWithBooks);
        }
    }
}
