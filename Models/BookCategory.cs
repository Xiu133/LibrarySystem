namespace Library.Models
{
    public class BookCategory
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public ICollection<Book>? Books { get; set; }
    }
}
