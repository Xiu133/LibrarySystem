using Library.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers.Account
{
    public class AccountController : Controller
    {
        private readonly SignInManager<User> _SignInManager;
        private readonly UserManager<User> _UserManager;

        public AccountController(SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _SignInManager = signInManager;
            _UserManager = userManager;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _UserManager.FindByNameAsync(username);
            if (user == null)
            {
                ModelState.AddModelError("", "帳號不存在");
                return View();
            }

            var result = await _SignInManager.PasswordSignInAsync(username, password, false, false);
            if (result.Succeeded)
            {
                var roles = await _UserManager.GetRolesAsync(user);

                if (roles.Contains("Admin"))
                {
                    return RedirectToAction("Index", "Admin", new { area = "" });
                }
                else
                {
                    return RedirectToAction("Index", "User", new { area = "" });
                }
            }

            ModelState.AddModelError("", "登入失敗");
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string username, string password)
        {
            var user = new User { UserName = username, Role = "User" };
            var result = await _UserManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                await _UserManager.AddToRoleAsync(user, "User");
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View("Register");
        }

        public async Task<IActionResult> Logout()
        {
            await _SignInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Manage()
        {
            var users = _UserManager.Users.Select(u => new User
            {
                Id = u.Id,
                UserName = u.UserName,
                Role = u.Role
            }).ToList();

            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> PromoteToAdmin(string userid)
        {
            var user = await _UserManager.FindByIdAsync(userid);
            if (user != null && !(await _UserManager.IsInRoleAsync(user, "Admin")))
            {
                await _UserManager.AddToRoleAsync(user, "Admin");
                await _UserManager.RemoveFromRoleAsync(user, "User");

                user.Role = "Admin";
                await _UserManager.UpdateAsync(user);
            }
            return RedirectToAction("Index", "Admin");
        }
    }
}
