using Library.Services.Interfaces;
using Library.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers.Finance
{
    [Authorize]
    public class FineController : Controller
    {
        private readonly IFineService _fineService;
        public FineController(IFineService fineService) => _fineService = fineService;

        private static FineViewModel ToViewModel(Library.Models.Fine f) => new()
        {
            Id = f.Id,
            UserName = f.UserName,
            BookTitle = f.BorrowRecord?.Book?.Title ?? "未知",
            Amount = f.Amount,
            IsPaid = f.IsPaid,
            PaidDate = f.PaidDate,
            CreatedAt = f.CreatedAt,
            BorrowDate = f.BorrowRecord?.BorrowDate,
            DueDate = f.BorrowRecord?.DueDate
        };

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index(int page = 1)
        {
            const int pageSize = 10;
            var allFines = (await _fineService.GetAllFinesAsync()).Select(ToViewModel).ToList();

            ViewBag.TotalUnpaidCount = allFines.Count(f => !f.IsPaid);
            ViewBag.TotalUnpaidAmount = allFines.Where(f => !f.IsPaid).Sum(f => f.Amount);
            ViewBag.TotalPaidCount = allFines.Count(f => f.IsPaid);

            var result = new PagedResult<FineViewModel>
            {
                Items = allFines.Skip((page - 1) * pageSize).Take(pageSize).ToList(),
                TotalCount = allFines.Count,
                Page = page,
                PageSize = pageSize
            };

            ViewBag.Page = page;
            ViewBag.TotalPages = result.TotalPages;
            ViewBag.PaginationAction = "Index";

            return View(result);
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
            return View(fines.Select(ToViewModel));
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
