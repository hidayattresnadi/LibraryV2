using SimpleLibraryV2.Models;
using WebAPITutorial.Repositories;

namespace SimpleLibraryV2.NewFolder
{
    public interface IBorrowingRepository : IRepository<Borrowing>
    {
        public void UpdateRange(IEnumerable<Borrowing> borrowings);
    }
}