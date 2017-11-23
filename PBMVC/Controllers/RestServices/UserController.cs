using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PicBook;
using PBMVC.Controllers.Services;

namespace PBMVC.Controllers.RestServices
{
    public class UserController: Controller
    {
        private IUserService userService;
        public UserController(IUserService userService)
        {
            this.userService = userService;

        }

        [HttpGet]
        public User GetUserInfo([FromQuery] Guid userID)
        {

            User user = userService.GetUserById(userID);
            if (user != null && user.IsActive)
            {
                user.UserIdentifier = null;
                user.SharedWith = null;
                return user;
            }

            return null;
        }

        [HttpPost]
        public void EditUser([FromBody]User user)
        {
            User oldUser = userService.LoggedIn(HttpContext);
            if (oldUser != null)
            {
                oldUser.Name = user.Name;
                oldUser.BirthDate = user.BirthDate;
                oldUser.Email = user.Email;
                userService.EditUser(oldUser);
            } else {
                throw new OperationNotPermittedException();
            }
        }
    }
}
