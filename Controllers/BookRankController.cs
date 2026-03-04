using Library.Data;
using Library.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BookRankController : Controller
    {
        private readonly LibrarydbContext _context;
        public BookRankController(LibrarydbContext context) { _context = context; }

        public async Task<IActionResult> Index()
        {
            var books = await _context.BorrowRecords
                .GroupBy(b => b.BookId)
                .Select(g => new { BookId = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .Take(10)
                .Join(_context.books,
                    br => br.BookId,
                    book => book.Id,
                    (br, book) => new PopularBookViewModel
                    {
                        BookId = book.Id,
                        Title = book.Title,
                        Author = book.Author,
                        BorrowCount = br.Count
                    })
                .ToListAsync();
            return View(books);
        }
    }
}
