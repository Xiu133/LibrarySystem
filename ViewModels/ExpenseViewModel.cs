namespace Library.ViewModels
{
    public class ExpenseViewModel
    {
        public int Id { get; set; }
        public string Category { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
    }
}
