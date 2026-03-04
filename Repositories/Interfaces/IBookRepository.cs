using Library.Models;

namespace Library.Repositories.Interfaces
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAllAsync();
        Task<Book?> GetByIdAsync(int id);
        Task<Book?> GetByTitleAsync(string title);
        Task<IEnumerable<Book>> SearchAsync(string query);
        Task AddOrIncrementAsync(Book book);
        Task<bool> UpdateAsync(Book book);
        Task SoftDeleteAsync(int id);
    }
}
