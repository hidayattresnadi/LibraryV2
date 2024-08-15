using SimpleLibraryV2.Context;
using SimpleLibraryV2.Models;
using WebAPITutorial.Repositories;

namespace SimpleLibraryV2.NewFolder
{
    public class BorrowingRepository : Repository<Borrowing>, IBorrowingRepository
    {
        private readonly MyDbContext _db;
        public BorrowingRepository(MyDbContext db) : base(db)
        {
            _db = db;
        }

        public void UpdateRange(IEnumerable<Borrowing> borrowings) {
            _db.UpdateRange(borrowings);
        }
    }
}