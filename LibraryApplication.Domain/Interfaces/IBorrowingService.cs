using SimpleLibraryV2.Models;

namespace SimpleLibraryV2.Interfaces
{
    public interface IBorrowingService
    {
        Task<List<Borrowing>> BorrowBook(BorrowingInput borrowingInput, int maximumBorrowedBook, int maximumLoanDays);
        Task<List<Borrowing>> ReturnBook(BorrowingInput borrowingInput);
    }
}
