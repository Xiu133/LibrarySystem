using Library.Models;
using Library.ViewModels;

namespace Library.Services.Interfaces
{
    public interface ISystemConfigService
    {
        Task<string?> GetAsync(string key);
        Task<ServiceResult> SetAsync(string key, string value, string? description = null);
        Task<IEnumerable<SystemConfig>> GetAllAsync();
    }
}
