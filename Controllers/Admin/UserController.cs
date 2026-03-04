using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers.Admin
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
