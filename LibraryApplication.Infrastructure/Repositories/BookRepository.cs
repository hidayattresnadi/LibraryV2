using LibraryApplication.Infrastructure.Repositories;
using SimpleLibraryV2.Context;
using SimpleLibraryV2.Models;
using WebAPITutorial.Repositories;

namespace SimpleLibraryV2.NewFolder
{
    public class BookRepository : Repository<Book>,IBookRepository
    {
        private readonly MyDbContext _db;
        public BookRepository(MyDbContext db) : base(db)
        {
            _db = db;
        }

        public Book Update(Book foundBook,Book book)
        {
            foundBook.Title = book.Title;
            foundBook.Author = book.Author;
            foundBook.PublicationYear = book.PublicationYear;
            foundBook.ISBN = book.ISBN;
            return foundBook;
        }
    }
}
