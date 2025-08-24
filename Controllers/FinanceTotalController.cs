using System.Security.Cryptography.Pkcs;
using Library.Data;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    public class FinanceTotalController : Controller
    {
        public readonly LibrarydbContext _context;

        public FinanceTotalController(LibrarydbContext context)
        {
            _context = context;
        }
        
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public IActionResult GetIncomeData()
        {
            var data = _context.incomeRecords
                .GroupBy(i => i.Reason)
                .Select(g => new
                {
                    Reason = g.Key,
                    Total = g.Sum(x => Convert.ToDecimal(x.Money))
                })
                .ToList();

            return Json(data);
        }
    }
}
