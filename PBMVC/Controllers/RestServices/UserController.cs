using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PicBook;
using PBMVC.Controllers.Services;

namespace PBMVC.Controllers.RestServices
{
    [Route("api/[controller]")]
    public class UserController: Controller
    {
        private IUserService userService;
        public UserController(IUserService userService)
        {
            this.userService = userService;

        }

        [HttpGet]
        public User GetUserInfo([FromQuery] Guid userId)
        {

            User user = userService.GetUserById(userId);
            if (user != null && user.IsActive)
            {
                user.UserIdentifier = null;
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
