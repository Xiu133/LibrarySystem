using System.Web;
using Library.Data;
using Library.Interface;
using Library.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookRepository _bookRepository;
        private readonly LibrarydbContext _dbContext;

        public BookController(IBookRepository bookRepository, LibrarydbContext dbContext)
        {
            _bookRepository = bookRepository;
            _dbContext = dbContext;
        }
        //瀏覽
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var books = await _bookRepository.GetAllBooksAsync();

            return View(books);
        }

        [HttpGet]
        public IActionResult Manage()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create() //新增
        {
            int newBookId = _dbContext.books.Any() ? _dbContext.books.Max(b => b.Id) + 1 : 1;
            ViewBag.NewBookId = newBookId;
            return View();
        }

        [HttpPost]
        public IActionResult Create(Book book)
        {
            var ImageFile = Request.Form.Files["ImageFile"]; // ✅ 手動取得檔案

            if (ModelState.IsValid)
            {
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "image");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    string originalFileName = Path.GetFileName(ImageFile.FileName);
                    string filePath = Path.Combine(uploadsFolder, originalFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        ImageFile.CopyTo(fileStream);
                    }

                    book.ImageFileName = $"/image/{originalFileName}";
                }
                else
                {
                    ModelState.AddModelError("ImageFile", "請上傳圖片");
                    return View(book);
                }

                _dbContext.books.Add(book);
                _dbContext.SaveChanges();

                return RedirectToAction("Index", "Admin");
            }

            return View(book);
        }

        [HttpGet]
        public IActionResult Edit(string title)//編輯
        {
            if (string.IsNullOrEmpty(title))
            {
                return View();
            }

            var book = _bookRepository.GetBookByTitle(title); 
            if (book == null) return NotFound(); 

            return View(book); 
        }

        [HttpPost]
        public async Task<IActionResult> Edit( Book book)//編輯頁面
        {
            if (book == null || book.Id == 0)
            {
                return NotFound();  // 防止 ID 為 0 或為空的情況
            }

            var existingBook = _bookRepository.GetBookById(book.Id);
            if (existingBook == null) return NotFound(); 

            if (ModelState.IsValid)
            {
                existingBook.Title = book.Title;
                existingBook.Author = book.Author;
                existingBook.Description = book.Description;
                existingBook.PublishedYear = book.PublishedYear;
                existingBook.Quantity = book.Quantity;

                await _bookRepository.UpdateBookAsync(existingBook);
                return RedirectToAction("Index", "Admin");
            }
            return View(book);
        }

        [HttpGet]
        public async Task<IActionResult> Delete() // 顯示刪除頁面
        {
            var book =  await _bookRepository.GetAllBooksAsync();
            return View(book);
        }

        [HttpPost]
        public IActionResult DeleteConfirm(int id)//刪除過程
        {
            var book =  _bookRepository.GetBookById(id);
            if (book == null) return NotFound();

            _bookRepository.SoftDeleteBookAsync(id);
            return RedirectToAction("Index", "Admin");
        }
        //搜尋功能
        [HttpGet]
        public async Task<IActionResult> Search(string? query) //回傳到 Views/Books/Search
        {                                                      
            var books = string.IsNullOrEmpty(query)
                ? await _dbContext.books.ToListAsync()  // 若無搜尋條件，則回傳所有書籍
                : await _dbContext.books
            .Where(b => b.Title.Contains(query) || b.Author.Contains(query))
            .ToListAsync();

            return Json(books);
        }

        [HttpGet]
        public ActionResult Detail(int id) //介紹
        {
            var book = _bookRepository.GetBookById(id);
            if (book == null)
            {
                return BadRequest("無效的書籍 ID");
            }

            return View(book);
        }
    }
}
