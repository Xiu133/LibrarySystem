using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _SignInManager;
        private readonly UserManager<IdentityUser> _UserManager;

        public AccountController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _SignInManager = signInManager;
            _UserManager = userManager;
        }

        public IActionResult Login() //留
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _UserManager.FindByNameAsync(username); // 查詢使用者
            if (user == null)
            {
                ModelState.AddModelError("", "帳號不存在");
                return View();
            }

            var result = await _SignInManager.PasswordSignInAsync(username, password, true, false);
            if (result.Succeeded)
            {
                var roles = await _UserManager.GetRolesAsync(user);

                if (roles.Contains("Admin"))
                {
                    return RedirectToAction("Index", "Admin");
                }

                else
                {
                    return RedirectToAction("Index", "User");
                }
            }

            ModelState.AddModelError("", "登入失敗");
            return View();
        }

        public IActionResult Register()//留
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string username, string password)
        {
            var user = new IdentityUser { UserName = username };
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
    }
}
