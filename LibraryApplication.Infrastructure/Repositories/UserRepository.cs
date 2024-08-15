using SimpleLibraryV2.Context;
using SimpleLibraryV2.Models;
using SimpleLibraryV2.NewFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPITutorial.Repositories;

namespace LibraryApplication.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly MyDbContext _db;
        public UserRepository(MyDbContext db) : base(db)
        {
            _db = db;
        }

        public User Update(User foundUser, User user)
        {
            foundUser.PhoneNumber = user.PhoneNumber;
            foundUser.Address = user.Address;
            foundUser.IDIdentity = user.IDIdentity;
            foundUser.Name = user.Name;
            foundUser.Email = user.Email;
            return foundUser;
        }
    }
}
