using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int PublishedYear { get; set; }
        public int Quantity { get; set; } = 1;
        public string ImageFileName { get; set; } = string.Empty;
        public bool IsDelete { get; set; } = false;
        public string? ISBN { get; set; }
        public int? CategoryId { get; set; }
        public string Location { get; set; } = "未分類";
        public string Condition { get; set; } = "Good";
        public BookCategory? Category { get; set; }

        [Timestamp]
        public required byte[] RowVersion { get; set; }

    }
}
