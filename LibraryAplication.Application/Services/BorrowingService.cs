using Microsoft.EntityFrameworkCore;
using SimpleLibraryV2.Context;
using SimpleLibraryV2.Interfaces;
using SimpleLibraryV2.Models;
using Microsoft.Extensions.DependencyInjection;
using SimpleLibraryV2.NewFolder;

namespace SimpleLibraryV2.Services
{
    public class BookManager
    {
        private readonly IServiceProvider _serviceProvider;
        public BookManager( IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public async Task<List<Borrowing>> BorrowBook(BorrowingInput borrowingInput, int maximumBorrowedBooks, int maximumLoanDays)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
                var _bookRepository = scope.ServiceProvider.GetRequiredService<IBookRepository>();
                var _borrowingRepository = scope.ServiceProvider.GetRequiredService<IBorrowingRepository>();
                List<Borrowing> borrowingList = new List<Borrowing>();
                var bookIds = borrowingInput.BookIds;
                var userId = borrowingInput.UserId;
                if (bookIds.Count > maximumBorrowedBooks)
                {
                    throw new ArgumentException("Maximum borrowed Books can only be " + maximumBorrowedBooks);
                }
                bool hasDuplicates = bookIds.Count != bookIds.Distinct().Count();
                if (hasDuplicates)
                {
                    throw new ArgumentException("Books data input should not be dupplicate");
                }
                var user = await _userRepository.GetFirstOrDefaultAsync(user => user.UserId == userId);
                if (user == null)
                {
                    throw new ArgumentException("User Id Is Invalid");
                }
                DateTime borrowedDate = DateTime.UtcNow;
                DateTime dueDate = DateTime.UtcNow.AddDays(maximumLoanDays);
                foreach (var bookId in bookIds)
                {
                    var book = await _bookRepository.GetFirstOrDefaultAsync(book => book.Id == bookId);
                    if (book == null)
                    {
                        throw new ArgumentException("Book is not found");
                    }
                    var bookBorrowing = await _borrowingRepository.GetFirstOrDefaultAsync(borrowing => borrowing.BookId == bookId && borrowing.ReturnDate == null);
                    if (bookBorrowing != null)
                    {
                        throw new ArgumentException("Book is already borrowed, please borrow another book");
                    }

                    Borrowing borrowing = new Borrowing
                    {
                        BookId = bookId,
                        UserId = userId,
                        BorrowedDate = borrowedDate,
                        DueDate = dueDate
                    };
                    borrowingList.Add(borrowing);
                }
                await _borrowingRepository.AddRangeAsync(borrowingList);
                await _borrowingRepository.SaveAsync();
                return borrowingList;
            }
        }
        public async Task<List<Borrowing>> ReturnBook(BorrowingInput borrowingInput)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
                var _bookRepository = scope.ServiceProvider.GetRequiredService<IBookRepository>();
                var _borrowingRepository = scope.ServiceProvider.GetRequiredService<IBorrowingRepository>();
                bool hasDuplicates = borrowingInput.BookIds.Count() != borrowingInput.BookIds.Distinct().Count();
                if (hasDuplicates)
                {
                    throw new ArgumentException("Books data input should not be duplicate");
                }
                DateTime returnedDate = DateTime.UtcNow;
                var userId = borrowingInput.UserId;
                var user = await _userRepository.GetFirstOrDefaultAsync(user => user.UserId == userId);
                if (user == null)
                {
                    throw new ArgumentException("User Id Is Invalid");
                }
                var borrowings = await _borrowingRepository.GetAllAsync(b => borrowingInput.BookIds.Contains(b.BookId) && b.UserId == borrowingInput.UserId);
                var borrowedBookIds = borrowings
                    .Where(b => b.ReturnDate == null)
                    .Select(b => b.BookId)
                    .ToHashSet();
                var inputBookIds = borrowingInput.BookIds.ToHashSet();
                if (!inputBookIds.IsSubsetOf(borrowedBookIds))
                {
                    throw new ArgumentException("One or more books have not been borrowed by this user or have been returned.");
                }
                var updatedBorrowings = new List<Borrowing>();
                foreach (var borrowing in borrowings)
                {
                    if (borrowing.ReturnDate == null) 
                    {
                        borrowing.ReturnDate = returnedDate;
                        updatedBorrowings.Add(borrowing);
                    }
                }
                _borrowingRepository.UpdateRange(borrowings);
                await _borrowingRepository.SaveAsync();
                return updatedBorrowings;
            }
        }
    }
}
