using Library.Data;
using Library.Interface;
using Library.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly LibrarydbContext _context;

        public BookRepository(LibrarydbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            return await _context.books.Where(b => !b.IsDelete).ToListAsync();
        }

        //public async Task<IEnumerable<Book>> GetAllBooksAsync()
        //{
        //    return await _context.books.ToListAsync();
        //}

        public async Task AddBookAsync(Book book)
        {
            await _context.books.AddAsync(book);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBookAsync(int id)
        {
            var book = await _context.books.FindAsync(id);
            if (book != null)
            {
                _context.books.Remove(book);
                await _context.SaveChangesAsync();
            }
        }

        public Book? GetBookById(int id)
        {
            return _context.books.FirstOrDefault(b => b.Id == id);
        }

        public Book? GetBookByTitle(string title)
        {
            return _context.books
                .FirstOrDefault(b => b.Title.ToLower() == title.ToLower());
        }

        public async Task<bool> UpdateBookAsync(Book book)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var existingBook = await _context.books.FindAsync(book.Id);
                    if (existingBook == null) return false;

                    // 只更新有改變的欄位
                    existingBook.Title = book.Title;
                    existingBook.Author = book.Author;
                    existingBook.Description = book.Description;
                    existingBook.PublishedYear = book.PublishedYear;
                    existingBook.Quantity = book.Quantity;

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return true;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task SoftDeleteBookAsync(int id)
        {
            var book = await _context.books.FindAsync(id);
            if(book != null)
            {
                book.IsDelete = true;
                await _context.SaveChangesAsync();
            }
        }
    }
}
