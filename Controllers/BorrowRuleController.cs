using Library.Data;
using Library.Models;
using Library.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Library.Controllers
{
    public class BorrowRuleController : Controller
    {
        public readonly LibrarydbContext _context;
        public readonly UserManager<User> _userManager;

        public BorrowRuleController(LibrarydbContext context , UserManager<User> userManager)
        {
            this._context = context;
            this._userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var roles = await _context.Users
        .Select(u => u.Role)
        .Distinct()
        .Where(r => !string.IsNullOrEmpty(r)) // 排除 null 或空字串
        .Select(r => new SelectListItem
        {
            Value = r,
            Text = r
        })
        .ToListAsync();

            var viewModel = new BorrowRuleViewModel
            {
                RoleList = roles
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SavePermisson(BorrowRuleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            var existingRule = await _context.BorrowRule.FirstOrDefaultAsync(r => r.Role == model.Role);
            if (existingRule != null)
            {
                // 更新
                existingRule.MaxBooks = model.MaxBooks;
                //existingRule.BorrowDays = model.BorrowDays;
            }
            else
            {
                // 新增
                var rule = new BorrowRule
                {
                    Role = model.Role,
                    MaxBooks = model.MaxBooks,
                    //BorrowDays = model.BorrowDays
                };
                _context.BorrowRule.Add(rule);
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = $"{model.Role} 的設定已儲存";
            return RedirectToAction("Index", "Admin");
        }

        [HttpGet]
        public async Task<IActionResult> GetBorrowRules(string role)
        {
            var rule = await _context.BorrowRule.FirstOrDefaultAsync(r => r.Role == role);
            if (rule == null)
            {
                return Json(new { maxBooks = 0, borrowDays = 0 });
            }

            return Json(new { maxBooks = rule.MaxBooks/*, borrowDays = rule.BorrowDays*/ });
        }

        [HttpGet]
        public async Task<IActionResult> GetUserBorrowRule()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            var rule = await _context.BorrowRule.FirstOrDefaultAsync(r => r.Role == user.Role);

            if (rule == null)
                return Json(new { maxBooks = 3, borrowDays = 3 }); // 預設值（可自行定義）

            return Json(new
            {
                maxBooks = rule.MaxBooks,
                //borrowDays = rule.BorrowDays
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetMaxBooks()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Json(new { maxBooks = 3 }); // 預設值
            }

            var roles = await _userManager.GetRolesAsync(user);
            string role = roles.FirstOrDefault() ?? "User"; // 取第一個角色

            var rule = await _context.BorrowRule.FirstOrDefaultAsync(r => r.Role == role);
            int maxBooks = rule?.MaxBooks ?? 3;

            return Json(new { maxBooks });
        }

    }
}
