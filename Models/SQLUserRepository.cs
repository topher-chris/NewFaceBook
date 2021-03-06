using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NewFaceBook.Models
{
    public class SQLUserRepository : IUserRepository
    {
        private readonly AppDbContext context;
        public SQLUserRepository(AppDbContext context)
        {
            this.context = context;
        }
        public User Add(User user)
        {
            context.Users.Add(user);
            context.SaveChanges();
            return user;
        }

        public User Delete(int id)
        {
            User user = context.Users.Find(id);
            if (user != null)
            {
            context.Users.Remove(user);
            context.SaveChanges();
            }
            return user;
        }

        public IEnumerable<User> GetAllUser()
        {
            return context.Users;
        }

        public User GetUser(int Id)
        {
            return context.Users.Find(Id);
        }

        public User Update(User userChanges)
        {
           var user = context.Users.Attach(userChanges);
            user.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return userChanges;
        }
    }
}
