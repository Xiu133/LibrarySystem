using Microsoft.AspNetCore.Mvc.Rendering;

namespace Library.ViewModel
{
    public class BorrowRuleViewModel
    {
        public string Role { get; set; } = string.Empty;
        public int MaxBooks { get; set; }
        public int BorrowDays { get; set; }

        public List<SelectListItem> RoleList { get; set; } = new(); 
    }
}
