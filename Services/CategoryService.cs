using Library.Models;
using Library.Repositories.Interfaces;
using Library.Services.Interfaces;
using Library.ViewModels;

namespace Library.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repo;
        public CategoryService(ICategoryRepository repo) => _repo = repo;

        public async Task<IEnumerable<BookCategory>> GetAllAsync() =>
            await _repo.GetAllAsync();

        public async Task<BookCategory?> GetByIdAsync(int id) =>
            await _repo.GetByIdAsync(id);

        public async Task<ServiceResult> CreateAsync(string name, string? description)
        {
            if (string.IsNullOrWhiteSpace(name))
                return ServiceResult.Fail("分類名稱不可為空。");
            await _repo.AddAsync(new BookCategory { Name = name, Description = description });
            return ServiceResult.Ok();
        }

        public async Task<ServiceResult> UpdateAsync(int id, string name, string? description)
        {
            var cat = await _repo.GetByIdAsync(id);
            if (cat == null) return ServiceResult.Fail("分類不存在。");
            cat.Name = name;
            cat.Description = description;
            await _repo.UpdateAsync(cat);
            return ServiceResult.Ok();
        }

        public async Task<ServiceResult> DeleteAsync(int id)
        {
            await _repo.DeleteAsync(id);
            return ServiceResult.Ok();
        }
    }
}
