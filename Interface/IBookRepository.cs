using Library.Models;

namespace Library.Interface
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAllBooksAsync();
        Book? GetBookById(int id);
        Task AddBookAsync(Book book);
        Task DeleteBookAsync(int id);
        Task<bool> UpdateBookAsync(Book book);

        Book? GetBookByTitle(string title);
        Task SoftDeleteBookAsync(int id);
    }
}
