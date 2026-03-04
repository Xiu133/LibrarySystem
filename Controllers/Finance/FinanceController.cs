using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers.Finance
{
    public class FinanceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
