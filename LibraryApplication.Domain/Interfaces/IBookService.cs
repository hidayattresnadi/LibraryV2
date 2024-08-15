using SimpleLibraryV2.Models;

namespace SimpleLibraryV2.Interfaces
{
    public interface IBookService
    {
        Task<Book> AddBook(Book book);
        Task<IEnumerable<Book>> GetAllBooks();
        Task<Book> GetBookById(int id);
        Task<Book> UpdateBook(Book book, int id);
        Task<bool> DeleteBook(int id);
    }
}
