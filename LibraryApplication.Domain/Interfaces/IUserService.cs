using SimpleLibraryV2.Models;

namespace SimpleLibraryV2.Interfaces
{
    public interface IUserService
    {
        Task<User> AddUser(User user);
        Task<IEnumerable<User>> GetAllUsers();
        Task<User> GetUserById(int id);
        Task<User> UpdateUser(User user, int id);
        Task<bool> DeleteUser(int id);
    }
}
