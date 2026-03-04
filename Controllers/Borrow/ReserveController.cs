using Library.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using static Library.Controllers.Borrow.BorrowController;

namespace Library.Controllers.Borrow
{
    public class ReserveController : Controller
    {
        private readonly IReserveService _reserveService;

        public ReserveController(IReserveService reserveService)
        {
            _reserveService = reserveService;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = User?.Identity?.Name ?? string.Empty;
            var books = await _reserveService.GetReservableBooksAsync(currentUser);
            return View(books);
        }

        public async Task<IActionResult> Reserve(int BookId)
        {
            var userName = User?.Identity?.Name;
            if (string.IsNullOrEmpty(userName))
                return RedirectToAction("Login", "Account");

            var result = await _reserveService.ReserveBookAsync(BookId, userName);

            if (!result.Success)
                TempData["ErrorMessage"] = result.Message;
            else
                TempData["Message"] = "預約成功！";

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmReserve([FromBody] BorrowRequest request)
        {
            if (request?.SelectBooks == null || !request.SelectBooks.Any())
                return Json(new { success = false, message = "請選擇要預約的書籍。" });

            var userName = User.Identity?.Name;
            if (string.IsNullOrEmpty(userName))
                return Json(new { success = false, message = "請先登入！" });

            var result = await _reserveService.ReserveBooksAsync(request.SelectBooks, userName);
            return Json(new
            {
                success = result.Success,
                message = result.Success ? "預約完成！" : result.Message,
                reservedBooks = result.Data
            });
        }
    }
}
