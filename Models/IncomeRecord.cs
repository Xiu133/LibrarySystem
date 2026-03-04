using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models
{
    public class IncomeRecord
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        [Column(TypeName = "decimal(10,2)")]
        public decimal Money { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}
