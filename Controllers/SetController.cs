using Library.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SetController : Controller
    {
        private readonly ISystemConfigService _configService;
        public SetController(ISystemConfigService configService) => _configService = configService;

        public async Task<IActionResult> Index()
        {
            var configs = await _configService.GetAllAsync();
            return View(configs);
        }

        [HttpPost]
        public async Task<IActionResult> Save(string key, string value)
        {
            var result = await _configService.SetAsync(key, value);
            return Json(new { success = result.Success });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var configs = await _configService.GetAllAsync();
            return Json(configs);
        }
    }
}
