using Library.Data;
using Library.Models;
using Library.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Library.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        private readonly LibrarydbContext _context;
        public MemberRepository(LibrarydbContext context) => _context = context;

        public async Task<IEnumerable<User>> GetAllAsync() =>
            await _context.Users.ToListAsync();

        public async Task<User?> GetByUserNameAsync(string userName) =>
            await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);

        public async Task<bool> UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
