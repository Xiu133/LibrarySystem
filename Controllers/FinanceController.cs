using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    public class FinanceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
