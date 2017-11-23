using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PicBook;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace PBMVC.Controllers.Services
{
    public interface IUserService
    {

        User GetUserById(Guid userid);
        void EditUser(User user);
        void CreateUser(User user);
        User LoggedIn(HttpContext context);

    }
}
