using Library.Data;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    public class IncomeRecordController : Controller
    {
        private readonly LibrarydbContext _context;

        public IncomeRecordController(LibrarydbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var records = _context.incomeRecords
                .OrderByDescending(x => x.PaymentDate)
                .ToList();
            return View(records);
        }
    }
}
