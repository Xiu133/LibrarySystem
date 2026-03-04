using Library.Models;
namespace Library.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<BookCategory>> GetAllAsync();
        Task<BookCategory?> GetByIdAsync(int id);
        Task AddAsync(BookCategory category);
        Task<bool> UpdateAsync(BookCategory category);
        Task DeleteAsync(int id);
    }
}
