using Library.Models;
namespace Library.Repositories.Interfaces
{
    public interface ISystemConfigRepository
    {
        Task<string?> GetValueAsync(string key);
        Task SetValueAsync(string key, string value, string? description = null);
        Task<IEnumerable<SystemConfig>> GetAllAsync();
    }
}
