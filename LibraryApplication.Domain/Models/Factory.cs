namespace SimpleLibraryV2.Models
{
    public class Factory
    {
        public static Book GetBook(Book book)
        {
            return new Book
            {
                Title = book.Title,
                Author = book.Author,
                PublicationYear = book.PublicationYear,
                ISBN = book.ISBN
            };
        }
        public static User GetUser(User user)
        {
            return new User
            {
                Name = user.Name,
                Address = user.Address,
                IDIdentity = user.IDIdentity,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email
            };
    }
    }
}
