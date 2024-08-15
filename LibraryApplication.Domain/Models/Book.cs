using System.ComponentModel.DataAnnotations;

namespace SimpleLibraryV2.Models
{
    public class Book
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Author is required")]
        public string Author { get; set; }
        [Required(ErrorMessage = "Publication Year is required")]
        public int PublicationYear { get; set; }
        [Required(ErrorMessage = "ISBN is required")]
        public string ISBN { get; set; }
    }
}
