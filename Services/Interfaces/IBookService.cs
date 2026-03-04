using Library.Models;
using Microsoft.AspNetCore.Http;

namespace Library.Services.Interfaces
{
    public interface IBookService
    {
        Task<IEnumerable<Book>> GetAllBooksAsync();
        Task<Book?> GetBookByIdAsync(int id);
        Task<Book?> GetBookByTitleAsync(string title);
        Task<IEnumerable<Book>> SearchBooksAsync(string query);
        Task CreateOrIncrementBookAsync(Book book, IFormFile? imageFile);
        Task<bool> UpdateBookAsync(Book book);
        Task SoftDeleteBookAsync(int id);
    }
}
