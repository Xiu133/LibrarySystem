using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models
{
    public class Fine
    {
        public int Id { get; set; }
        public int BorrowRecordId { get; set; }
        public string UserName { get; set; } = string.Empty;
        [Column(TypeName = "decimal(10,2)")]
        public decimal Amount { get; set; }
        public bool IsPaid { get; set; }
        public DateTime? PaidDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public BorrowRecord? BorrowRecord { get; set; }
    }
}
