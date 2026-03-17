using Library.Data;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers.Finance
{
    public class IncomeRecordController : Controller
    {
        private readonly LibrarydbContext _context;

        public IncomeRecordController(LibrarydbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int page = 1)
        {
            const int pageSize = 10;
            var query = _context.incomeRecords.OrderByDescending(x => x.PaymentDate);

            var totalCount = query.Count();
            var records = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.Page = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            ViewBag.PaginationAction = "Index";

            return View(records);
        }
    }
}
