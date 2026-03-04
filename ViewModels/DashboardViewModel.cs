namespace Library.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalBooks { get; set; }
        public int TotalMembers { get; set; }
        public int ActiveBorrows { get; set; }
        public int OverdueCount { get; set; }
        public int UnpaidFines { get; set; }
        public decimal TotalIncome { get; set; }
        public decimal TotalExpense { get; set; }
        public List<MonthlyStatItem> MonthlyBorrows { get; set; } = new();
        public List<RecentActivityItem> RecentActivities { get; set; } = new();
    }

    public class MonthlyStatItem
    {
        public string Month { get; set; } = string.Empty;
        public int Count { get; set; }
    }

    public class RecentActivityItem
    {
        public string UserName { get; set; } = string.Empty;
        public string BookTitle { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public DateTime Date { get; set; }
    }
}
