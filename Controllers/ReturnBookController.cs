using Library.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    public class ReturnBookController : Controller
    {
        private readonly IBorrowService _borrowService;

        public ReturnBookController(IBorrowService borrowService)
        {
            _borrowService = borrowService;
        }

        public IActionResult Index() => View();

        [HttpGet]
        public async Task<IActionResult> GetActiveRecords(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                return Json(new { success = false, message = "請輸入帳號" });

            var records = await _borrowService.GetActiveRecordsByUserAsync(userName);
            var data = records.Select(br => new
            {
                borrowRecordId = br.Id,
                title = br.Book?.Title ?? "未知",
                author = br.Book?.Author ?? "未知",
                borrowDate = br.BorrowDate.ToString("yyyy-MM-dd")
            });
            return Json(new { success = true, records = data });
        }

        [HttpPost]
        public async Task<IActionResult> ReturnBook(int borrowRecordId)
        {
            var result = await _borrowService.ReturnBookAsync(borrowRecordId);
            return Json(new { success = result.Success, message = result.Message });
        }
    }
}
