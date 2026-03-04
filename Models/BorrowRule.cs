using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models
{
    public class BorrowRule
    {
        [Key]
        public int Id { get; set; }
        public string Role { get; set; } = string.Empty;
        public int MaxBooks { get; set; }
        public int BorrowDays { get; set; } = 14;
        [Column(TypeName = "decimal(10,2)")]
        public decimal FinePerDay { get; set; } = 5;
    }
}
