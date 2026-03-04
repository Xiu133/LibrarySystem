using Library.Models;
using Library.Services.Interfaces;
using Library.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers.Books
{
    public class BookController : Controller
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var books = await _bookService.GetAllBooksAsync();
            return View(books);
        }

        [HttpGet]
        public IActionResult Manage()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Book book)
        {
            var imageFile = Request.Form.Files["ImageFile"];

            if (imageFile == null || imageFile.Length == 0)
            {
                ModelState.AddModelError("ImageFile", "請上傳圖片");
                return View(book);
            }

            if (!ModelState.IsValid)
                return View(book);

            await _bookService.CreateOrIncrementBookAsync(book, imageFile);
            return RedirectToAction("Index", "Admin");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string title)
        {
            if (string.IsNullOrEmpty(title))
                return View();

            var book = await _bookService.GetBookByTitleAsync(title);
            if (book == null) return NotFound();

            return View(book);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Book book)
        {
            if (book == null || book.Id == 0)
                return NotFound();

            if (!ModelState.IsValid)
                return View(book);

            await _bookService.UpdateBookAsync(book);
            return RedirectToAction("Index", "Admin");
        }

        [HttpGet]
        public async Task<IActionResult> Delete()
        {
            var books = await _bookService.GetAllBooksAsync();
            return View(books);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            await _bookService.SoftDeleteBookAsync(id);
            return RedirectToAction("Index", "Admin");
        }

        [HttpGet]
        public async Task<IActionResult> Search(string? query)
        {
            if (string.IsNullOrEmpty(query))
            {
                var all = await _bookService.GetAllBooksAsync();
                return Json(all);
            }

            var books = await _bookService.SearchBooksAsync(query);
            return Json(books);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null)
                return BadRequest("無效的書籍 ID");

            return View(book);
        }

        public async Task<IActionResult> SearchResult(string query)
        {
            if (string.IsNullOrEmpty(query))
                return View(new List<BookViewModel>());

            var books = await _bookService.SearchBooksAsync(query);
            var viewModels = books.Select(b => new BookViewModel
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                PublishedYear = b.PublishedYear
            }).ToList();

            return View(viewModels);
        }

        [HttpGet]
        public async Task<IActionResult> GetDetail(int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null) return Json(null);
            return Json(new
            {
                id = book.Id,
                title = book.Title,
                author = book.Author,
                publishedYear = book.PublishedYear,
                quantity = book.Quantity,
                description = book.Description,
                imageFileName = book.ImageFileName,
                isbn = book.ISBN,
                location = book.Location
            });
        }

        public async Task<IActionResult> BorrowSearch(string query)
        {
            var books = await _bookService.SearchBooksAsync(query);
            return Json(books.Select(b => new
            {
                id = b.Id,
                title = b.Title,
                author = b.Author,
                quantity = b.Quantity
            }));
        }
    }
}
