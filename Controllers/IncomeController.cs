using Library.Data;
using Library.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.Controllers
{
    public class IncomeController : Controller
    {
        private readonly LibrarydbContext _context;
        private readonly UserManager<User> _userManager;

        public IncomeController(LibrarydbContext context,UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Income(string userName)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == userName);
            if (user == null)
            {
                ViewBag.UserConfirmed = false;
                ViewBag.Error = "查無此使用者";
                return View();
            }

            ViewBag.UserConfirmed = true;
            ViewBag.UserName = userName;
            return View();
        }


        [HttpPost]
        public IActionResult RecordPayment(string userName, string BookId, string Money, string Reason)
        {
            var payment = new IncomeRecord
            {
                UserName = userName,
                Reason = Reason,
                Money = Money,
                PaymentDate = DateTime.Now
            };

            _context.incomeRecords.Add(payment);
            _context.SaveChanges();

            TempData["Message"] = "✅ 收款成功！";
            return RedirectToAction("Index", "Admin"); 
        }     
    }
}
