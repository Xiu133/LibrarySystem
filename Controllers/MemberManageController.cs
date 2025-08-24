using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    public class MemberManageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
