using Library.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers.Admin
{
    public class NotifyController : Controller
    {
        private readonly INotifyService _notifyService;

        public NotifyController(INotifyService notifyService)
        {
            _notifyService = notifyService;
        }

        public async Task<IActionResult> Index()
        {
            var userName = User?.Identity?.Name;
            if (string.IsNullOrEmpty(userName))
                return View(Enumerable.Empty<object>());

            var notifications = await _notifyService.GetNotificationsForUserAsync(userName);
            return View(notifications);
        }
    }
}
