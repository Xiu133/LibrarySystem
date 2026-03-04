using Library.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    public class BorrowController : Controller
    {
        private readonly IBorrowService _borrowService;

        public BorrowController(IBorrowService borrowService)
        {
            _borrowService = borrowService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Borrow(int bookId)
        {
            var userName = User?.Identity?.Name;
            if (string.IsNullOrEmpty(userName))
                return RedirectToAction("Login", "Account");

            var result = await _borrowService.BorrowBookAsync(bookId, userName);
            if (!result.Success)
                TempData["ErrorMessage"] = result.Message;

            return RedirectToAction("Index");
        }

        public class BorrowRequest
        {
            public List<int>? SelectBooks { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmBorrow([FromBody] BorrowRequest request)
        {
            var userName = User.Identity?.Name;
            if (string.IsNullOrEmpty(userName))
                return Json(new { success = false, message = "請先登入！" });

            if (request?.SelectBooks == null || !request.SelectBooks.Any())
                return Json(new { success = false, message = "請選擇書籍。" });

            var result = await _borrowService.BorrowBooksAsync(request.SelectBooks, userName);
            return Json(new
            {
                success = result.Success,
                message = result.Success ? "借閱成功！" : result.Message,
                borrowedBooks = result.Data
            });
        }
    }
}
