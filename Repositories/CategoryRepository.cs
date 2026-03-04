using Library.Data;
using Library.Models;
using Library.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Library.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly LibrarydbContext _context;
        public CategoryRepository(LibrarydbContext context) => _context = context;

        public async Task<IEnumerable<BookCategory>> GetAllAsync() =>
            await _context.bookCategories.ToListAsync();

        public async Task<BookCategory?> GetByIdAsync(int id) =>
            await _context.bookCategories.FindAsync(id);

        public async Task AddAsync(BookCategory category)
        {
            await _context.bookCategories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(BookCategory category)
        {
            _context.bookCategories.Update(category);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task DeleteAsync(int id)
        {
            var cat = await _context.bookCategories.FindAsync(id);
            if (cat != null)
            {
                _context.bookCategories.Remove(cat);
                await _context.SaveChangesAsync();
            }
        }
    }
}
