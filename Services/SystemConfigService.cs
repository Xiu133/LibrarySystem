using Library.Models;
using Library.Repositories.Interfaces;
using Library.Services.Interfaces;
using Library.ViewModels;

namespace Library.Services
{
    public class SystemConfigService : ISystemConfigService
    {
        private readonly ISystemConfigRepository _repo;
        public SystemConfigService(ISystemConfigRepository repo) => _repo = repo;

        public async Task<string?> GetAsync(string key) =>
            await _repo.GetValueAsync(key);

        public async Task<ServiceResult> SetAsync(string key, string value, string? description = null)
        {
            await _repo.SetValueAsync(key, value, description);
            return ServiceResult.Ok();
        }

        public async Task<IEnumerable<SystemConfig>> GetAllAsync() =>
            await _repo.GetAllAsync();
    }
}
