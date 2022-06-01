using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewFaceBook.Models
{
    public interface IUserRepository
    {
        User GetUser(int Id);
        IEnumerable<User> GetAllUser();
        User Add(User user);
        User Update(User userChanges);
        User Delete(int id);
    }
}
