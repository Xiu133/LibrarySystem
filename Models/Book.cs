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
    }
}
