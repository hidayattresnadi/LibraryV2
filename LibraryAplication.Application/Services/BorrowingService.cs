using Microsoft.EntityFrameworkCore;
using SimpleLibraryV2.Context;
using SimpleLibraryV2.Interfaces;
using SimpleLibraryV2.Models;
using Microsoft.Extensions.DependencyInjection;

namespace SimpleLibraryV2.Services
{
    public class BorrowingService :IBorrowingService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public BorrowingService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }
        public async Task<List<Borrowing>> BorrowBook(BorrowingInput borrowingInput, int maximumBorrowedBooks, int maximumLoanDays)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                /// Mendapatkan DbContext scoped dari service provider scope baru
                var context = scope.ServiceProvider.GetRequiredService<MyDbContext>();
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
                var user = await context.Users.FirstOrDefaultAsync(user => user.UserId == userId);
                if (user == null)
                {
                    throw new ArgumentException("User Id Is Invalid");
                }
                DateTime borrowedDate = DateTime.UtcNow;
                DateTime dueDate = DateTime.UtcNow.AddDays(maximumLoanDays);
                foreach (var bookId in bookIds)
                {
                    var book = await context.Books.FirstOrDefaultAsync(book => book.Id == bookId);
                    if (book == null)
                    {
                        throw new ArgumentException("Book is not found");
                    }
                    var bookBorrowing = await context.Borrowings.FirstOrDefaultAsync(borrowing => borrowing.BookId == bookId && borrowing.ReturnDate == null);
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
                await context.Borrowings.AddRangeAsync(borrowingList);
                await context.SaveChangesAsync();
                return borrowingList;
            }
        }
        public async Task<List<Borrowing>> ReturnBook(BorrowingInput borrowingInput)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<MyDbContext>();
                bool hasDuplicates = borrowingInput.BookIds.Count() != borrowingInput.BookIds.Distinct().Count();
                if (hasDuplicates)
                {
                    throw new ArgumentException("Books data input should not be duplicate");
                }
                DateTime returnedDate = DateTime.UtcNow;
                var userId = borrowingInput.UserId;
                var user = await context.Users.FirstOrDefaultAsync(user => user.UserId == userId);
                if (user == null)
                {
                    throw new ArgumentException("User Id Is Invalid");
                }
                // Fetch all borrowings for the specified book IDs and user
                var borrowings = await context.Borrowings
                    .Where(b => borrowingInput.BookIds.Contains(b.BookId) && b.UserId == borrowingInput.UserId)
                    .ToListAsync();

                // Track books that are still on loan
                var borrowedBookIds = borrowings
                    .Where(b => b.ReturnDate == null)  // Only include books still on loan
                    .Select(b => b.BookId)
                    .ToHashSet();

                var inputBookIds = borrowingInput.BookIds.ToHashSet();

                // Validate if all books the user wants to borrow are currently on loan
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
                context.Borrowings.UpdateRange(borrowings);
                await context.SaveChangesAsync();
                return updatedBorrowings;
            }
        }
    }
}
