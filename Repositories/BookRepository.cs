using Library.Data;
using Library.Models;
using Library.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Library.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly LibrarydbContext _context;

        public BookRepository(LibrarydbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            return await _context.books.Where(b => !b.IsDelete).ToListAsync();
        }

        public async Task<Book?> GetByIdAsync(int id)
        {
            return await _context.books.FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Book?> GetByTitleAsync(string title)
        {
            return await _context.books
                .FirstOrDefaultAsync(b => b.Title.ToLower() == title.ToLower());
        }

        public async Task<IEnumerable<Book>> SearchAsync(string query)
        {
            return await _context.books
                .Where(b => !b.IsDelete &&
                    (b.Title.Contains(query) || b.Author.Contains(query)))
                .ToListAsync();
        }

        public async Task AddOrIncrementAsync(Book book)
        {
            var existing = await _context.books
                .FirstOrDefaultAsync(b => b.Title == book.Title && b.Author == book.Author);

            if (existing != null)
            {
                existing.Quantity += book.Quantity;
                existing.ImageFileName = book.ImageFileName;
                _context.books.Update(existing);
            }
            else
            {
                await _context.books.AddAsync(book);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(Book book)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var existing = await _context.books.FindAsync(book.Id);
                if (existing == null) return false;

                existing.Title = book.Title;
                existing.Author = book.Author;
                existing.Description = book.Description;
                existing.PublishedYear = book.PublishedYear;
                existing.Quantity = book.Quantity;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task SoftDeleteAsync(int id)
        {
            var book = await _context.books.FindAsync(id);
            if (book != null)
            {
                book.IsDelete = true;
                await _context.SaveChangesAsync();
            }
        }
    }
}
