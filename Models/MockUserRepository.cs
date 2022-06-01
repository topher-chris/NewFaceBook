using NewFaceBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewFaceBook.Models
{
    public class MockUserRepository : IUserRepository
    {
        private List<User> _userList;

        public MockUserRepository()
        {
            _userList = new List<User>()
            {
                new User() { Id = 1, Name = "Mummy", Location = Dept.West, Email = "mummy@yahoo.com" },
                new User() { Id = 2, Name = "John", Location = Dept.North, Email = "john@gmail.com"},
                new User() { Id = 3, Name = "Sam", Location = Dept.East, Email = "sam@prag.org"}
            };
        }

        public User Add(User user)
        {
            user.Id = _userList.Max(e => e.Id) + 1;
            _userList.Add(user);
            return user;
        }

        public User Delete(int id)
        {
            User user = _userList.FirstOrDefault(e => e.Id == id);
            if (user != null)
            {
                _userList.Remove(user);
            }
            return user;
        }

        public IEnumerable<User> GetAllUser()
        {
            return _userList;
        }

        public User GetUser(int Id)
        {
            return _userList.FirstOrDefault(e => e.Id == Id);
        }

        public User Update(User userChanges)
        {
            User user = _userList.FirstOrDefault(e => e.Id == userChanges.Id);
            if (user != null)
            {
                user.Name = userChanges.Name;
                user.Email = userChanges.Email;
                user.Location = userChanges.Location;
            }
            return user;
        }
    }
}

