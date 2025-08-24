using Library.Data;
using Library.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.Controllers
{
    public class NotifyController : Controller
    {
        private readonly LibrarydbContext _context;
        private readonly UserManager<User> _userManager;

        public NotifyController(LibrarydbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userName = User?.Identity?.Name;
            var notifications = await _context.notifies
                .Where(n => n.UserName == userName)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();

            return View(notifications);
        }
    }
}
