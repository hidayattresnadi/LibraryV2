using SimpleLibraryV2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPITutorial.Repositories;

namespace SimpleLibraryV2.NewFolder
{
    public interface IUserRepository : IRepository<User>
    {
        User Update(User foundUser, User user);
    }
}
