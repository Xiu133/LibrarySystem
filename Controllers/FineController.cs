using Library.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    [Authorize]
    public class FineController : Controller
    {
        private readonly IFineService _fineService;
        public FineController(IFineService fineService) => _fineService = fineService;

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var fines = await _fineService.GetAllFinesAsync();
            return View(fines);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Scan()
        {
            await _fineService.ScanAndCreateFinesAsync();
            TempData["Success"] = "逾期掃描完成！";
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> MarkPaid(int id)
        {
            var result = await _fineService.MarkAsPaidAsync(id);
            return Json(new { success = result.Success, message = result.Message });
        }

        public async Task<IActionResult> MyFines()
        {
            var userName = User.Identity?.Name ?? string.Empty;
            var fines = await _fineService.GetFinesByUserAsync(userName);
            return View(fines);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            var fines = await _fineService.GetAllFinesAsync();
            return Json(fines.Select(f => new
            {
                f.Id,
                f.UserName,
                BookTitle = f.BorrowRecord?.Book?.Title ?? "未知",
                f.Amount,
                f.IsPaid,
                f.PaidDate,
                f.CreatedAt
            }));
        }
    }
}
