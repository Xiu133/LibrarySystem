using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
    public class ReserveRecord
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public DateTime ReserveDate { get; set; }
        public bool IsNotify { get; set; } = false;

        [Timestamp] 
        public  byte[]? RowVersion { get; set; }
    }
}
