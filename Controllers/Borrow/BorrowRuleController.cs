using Library.Models;
using Library.Services.Interfaces;
using Library.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Library.Controllers.Borrow
{
    public class BorrowRuleController : Controller
    {
        private readonly IBorrowRuleService _borrowRuleService;
        private readonly UserManager<User> _userManager;

        public BorrowRuleController(IBorrowRuleService borrowRuleService, UserManager<User> userManager)
        {
            _borrowRuleService = borrowRuleService;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var roles = await _borrowRuleService.GetDistinctRolesAsync();
            var viewModel = new BorrowRuleViewModel
            {
                RoleList = roles.Select(r => new SelectListItem { Value = r, Text = r }).ToList()
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SavePermisson(BorrowRuleViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Index", model);

            await _borrowRuleService.SaveBorrowRuleAsync(model.Role, model.MaxBooks);
            TempData["Success"] = $"{model.Role} 的設定已儲存";
            return RedirectToAction("Index", "Admin");
        }

        [HttpGet]
        public async Task<IActionResult> GetBorrowRules(string role)
        {
            var rule = await _borrowRuleService.GetRuleByRoleAsync(role);
            return Json(new { maxBooks = rule?.MaxBooks ?? 0 });
        }

        [HttpGet]
        public async Task<IActionResult> GetUserBorrowRule()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var maxBooks = await _borrowRuleService.GetMaxBooksForRoleAsync(user.Role ?? "User");
            return Json(new { maxBooks });
        }

        [HttpGet]
        public async Task<IActionResult> GetMaxBooks()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Json(new { maxBooks = 3 });

            var roles = await _userManager.GetRolesAsync(user);
            string role = roles.FirstOrDefault() ?? "User";

            var maxBooks = await _borrowRuleService.GetMaxBooksForRoleAsync(role);
            return Json(new { maxBooks });
        }
    }
}
