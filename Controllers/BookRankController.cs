using Library.Data;
using Library.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.Controllers
{
    public class BookRankController : Controller
    {
        private readonly LibrarydbContext _context;

        public BookRankController(LibrarydbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int BookId)
        {
            var book = _context.BorrowRecords.GroupBy(b =>  b.BookId).Select(g => new
            {
                BookId = g.Key,
                Count = g.Count()
            })
        .OrderByDescending(x => x.Count)
        .Take(5)
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
        .ToList();

            return View(book);
        }
    }
}
