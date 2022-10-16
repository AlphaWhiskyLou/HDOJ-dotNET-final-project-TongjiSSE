using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModels.Models;

namespace AdminService
{
    public class AdminUserService
    {
        private compilerContext _compilerContext = new compilerContext();

        public async Task<List<User>> GetUserInfo()
        {
            var context = _compilerContext;
            IQueryable<User> users =
                from user in context.Users select user;
            return users.ToList();

        }

        public async Task<bool> UpdateUser(User user)
        {
            User userUpdate =
               _compilerContext.Users.First(c => c.UserId == user.UserId);

            userUpdate.UserId = user.UserId;
            userUpdate.Username = user.Username;
            userUpdate.Type = user.Type;
            userUpdate.University = user.University;


            _compilerContext.SaveChanges();

            return true;

        }

        public async Task<bool> DeleteUser(User user)
        {
            User userDelete =
              _compilerContext.Users.First(c => c.UserId == user.UserId);

            _compilerContext.Users.Remove(userDelete);
            _compilerContext.SaveChanges();

            return true;
        }

    }
}
