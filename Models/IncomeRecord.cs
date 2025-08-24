namespace Library.Models
{
    public class IncomeRecord
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public string Money { get; set; } = string.Empty;
        public DateTime PaymentDate { get; set; }
    }
}
