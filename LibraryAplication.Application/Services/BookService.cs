using Microsoft.EntityFrameworkCore;
using SimpleLibraryV2.Context;
using SimpleLibraryV2.Models;
using SimpleLibraryV2.Interfaces;
using SimpleLibraryV2.NewFolder;

namespace SimpleLibraryAPI.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }
        public async Task<Book> AddBook(Book inputBook)
        {
            var book = await _bookRepository.GetFirstOrDefaultAsync(book => book.Title == inputBook.Title);
            if (book != null) 
            {
                return null;
            }
            await _bookRepository.AddAsync(inputBook);
            await _bookRepository.SaveAsync();
            return inputBook;
        }
        public async Task<IEnumerable<Book>> GetAllBooks()
        {
            return await _bookRepository.GetAllAsync();
        }
        public async Task<Book> GetBookById(int id)
        {
            Book chosenBook = await _bookRepository.GetFirstOrDefaultAsync(foundBook => foundBook.Id == id);
            return chosenBook;
        }
        public async Task<Book> UpdateBook(Book book, int id)
        {
            var foundBook = await GetBookById(id);
            if (foundBook is null)
            {
                return null;
            }
            var updatedBook = _bookRepository.Update(foundBook,book);
            await _bookRepository.SaveAsync();
            return foundBook;
        }
        public async Task<bool> DeleteBook(int id)
        {
            var foundBook = await GetBookById(id);
            if (foundBook is null)
            {
                return false;
            }
            _bookRepository.Remove(foundBook);
            await _bookRepository.SaveAsync();
            return true;
        }
    }
}
