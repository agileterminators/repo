using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PicBook;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace PBMVC.Controllers.Services.Impl
{
    public class UserServiceImpl: DbContext, IUserService
    {

        private const String LOGGED_IN_USER = "LOGGED_IN_USER";

        private DbSet<User> Users { get; set; }
        
        public User GetUserById(Guid userid) {
            return Users.Find(userid);
        }
        public void EditUser(User user) {
            Entry(user).State = EntityState.Modified;

            SaveChanges();
        }
        public void CreateUser(User user) {
            Users.Add(user);

            SaveChanges();
        }
        public User LoggedIn(HttpContext context) {
            string userName = context.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            return Users.First(x => x.Name.Equals(userName));
        }

    }
}
