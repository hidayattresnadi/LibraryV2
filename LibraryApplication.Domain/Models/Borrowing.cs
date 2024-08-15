using System.ComponentModel.DataAnnotations;

namespace SimpleLibraryV2.Models
{
    public class Borrowing
    {
        [Key]
        public int BorrowingId { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public DateTime BorrowedDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}
