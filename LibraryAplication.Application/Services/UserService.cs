using LibraryApplication.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using SimpleLibraryV2.Context;
using SimpleLibraryV2.Interfaces;
using SimpleLibraryV2.Models;
using SimpleLibraryV2.NewFolder;

namespace SimpleLibraryV2.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<User> AddUser(User inputUser)
        {
            var user = await _userRepository.GetFirstOrDefaultAsync(user => user.Name == inputUser.Name);
            if (user != null)
            {
                return null;
            }
            await _userRepository.AddAsync(inputUser);
            await _userRepository.SaveAsync();
            return inputUser;
        }
        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _userRepository.GetAllAsync();
        }
        public async Task<User> GetUserById(int id)
        {
            User chosenUser = await _userRepository.GetFirstOrDefaultAsync(foundUser => foundUser.UserId == id);
            return chosenUser;
        }
        public async Task<User> UpdateUser(User user, int id)
        {
            var foundUser = await GetUserById(id);
            if (foundUser is null)
            {
                return null;
            }
            var updatedUser = _userRepository.Update(foundUser, user);
            await _userRepository.SaveAsync();
            return foundUser;
        }
        public async Task<bool> DeleteUser(int id)
        {
            var foundUser = await GetUserById(id);
            if (foundUser is null)
            {
                return false;
            }
            _userRepository.Remove(foundUser);
            await _userRepository.SaveAsync();
            return true;
        }
    }
}
