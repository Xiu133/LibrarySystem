using Library.Models;
using Library.Repositories.Interfaces;
using Library.Services.Interfaces;

namespace Library.Services
{
    public class BorrowRuleService : IBorrowRuleService
    {
        private const int DefaultMaxBooks = 3;
        private readonly IBorrowRuleRepository _ruleRepository;

        public BorrowRuleService(IBorrowRuleRepository ruleRepository)
        {
            _ruleRepository = ruleRepository;
        }

        public async Task<int> GetMaxBooksForRoleAsync(string role)
        {
            var rule = await _ruleRepository.GetByRoleAsync(role);
            return rule?.MaxBooks ?? DefaultMaxBooks;
        }

        public async Task SaveBorrowRuleAsync(string role, int maxBooks)
        {
            var existing = await _ruleRepository.GetByRoleAsync(role);
            if (existing != null)
            {
                existing.MaxBooks = maxBooks;
                await _ruleRepository.UpdateAsync(existing);
            }
            else
            {
                await _ruleRepository.AddAsync(new BorrowRule { Role = role, MaxBooks = maxBooks });
            }
        }

        public async Task<BorrowRule?> GetRuleByRoleAsync(string role)
        {
            return await _ruleRepository.GetByRoleAsync(role);
        }

        public async Task<IEnumerable<string>> GetDistinctRolesAsync()
        {
            return await _ruleRepository.GetDistinctRolesAsync();
        }
    }
}
