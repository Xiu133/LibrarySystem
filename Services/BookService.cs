using Library.Models;
using Library.Repositories.Interfaces;
using Library.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Library.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            return await _bookRepository.GetAllAsync();
        }

        public async Task<Book?> GetBookByIdAsync(int id)
        {
            return await _bookRepository.GetByIdAsync(id);
        }

        public async Task<Book?> GetBookByTitleAsync(string title)
        {
            return await _bookRepository.GetByTitleAsync(title);
        }

        public async Task<IEnumerable<Book>> SearchBooksAsync(string query)
        {
            return await _bookRepository.SearchAsync(query);
        }

        public async Task CreateOrIncrementBookAsync(Book book, IFormFile? imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "image");
                Directory.CreateDirectory(uploadsFolder);

                string fileName = Path.GetFileName(imageFile.FileName);
                string filePath = Path.Combine(uploadsFolder, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await imageFile.CopyToAsync(stream);

                book.ImageFileName = $"/image/{fileName}";
            }

            await _bookRepository.AddOrIncrementAsync(book);
        }

        public async Task<bool> UpdateBookAsync(Book book)
        {
            return await _bookRepository.UpdateAsync(book);
        }

        public async Task SoftDeleteBookAsync(int id)
        {
            await _bookRepository.SoftDeleteAsync(id);
        }
    }
}
