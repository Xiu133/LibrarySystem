using Library.Data;
using Library.Models;
using Library.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Library.Repositories
{
    public class SystemConfigRepository : ISystemConfigRepository
    {
        private readonly LibrarydbContext _context;
        public SystemConfigRepository(LibrarydbContext context) => _context = context;

        public async Task<string?> GetValueAsync(string key) =>
            (await _context.systemConfigs.FirstOrDefaultAsync(c => c.Key == key))?.Value;

        public async Task SetValueAsync(string key, string value, string? description = null)
        {
            var config = await _context.systemConfigs.FirstOrDefaultAsync(c => c.Key == key);
            if (config != null)
            {
                config.Value = value;
                if (description != null) config.Description = description;
                _context.systemConfigs.Update(config);
            }
            else
            {
                await _context.systemConfigs.AddAsync(new SystemConfig { Key = key, Value = value, Description = description });
            }
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<SystemConfig>> GetAllAsync() =>
            await _context.systemConfigs.ToListAsync();
    }
}
