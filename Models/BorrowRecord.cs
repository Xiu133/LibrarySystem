namespace Library.Models
{
    public class BorrowRecord
    {
        public int Id { get; set; }

        public int BookId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public DateTime BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public DateTime? DueDate { get; set; }
        public int RenewCount { get; set; } = 0;
        public Book? Book { get; set; }

    }
}
