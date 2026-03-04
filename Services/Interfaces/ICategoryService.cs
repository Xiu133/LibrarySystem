using Library.Models;
using Library.ViewModels;

namespace Library.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<BookCategory>> GetAllAsync();
        Task<BookCategory?> GetByIdAsync(int id);
        Task<ServiceResult> CreateAsync(string name, string? description);
        Task<ServiceResult> UpdateAsync(int id, string name, string? description);
        Task<ServiceResult> DeleteAsync(int id);
    }
}
