using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace PBMVC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult LoginFacebook() 
        {
            var authenticationProperties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("AuthCallback", "Home")
            };

            return Challenge(authenticationProperties, "Facebook");
        }

        public IActionResult AuthCallback()
        {
            var facebookIdentity = User.Identities.FirstOrDefault(i => i.AuthenticationType == "Facebook" && i.IsAuthenticated);

            if (facebookIdentity == null)
            {
                return Redirect(Url.Action("Login", "Account"));
            }

            // facebookIdentity.Claims // TODO: <--- user adatot megkapjuk a claim-el lehet feldolgozni
            
            return Redirect(Url.Action("Index", "Home"));
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return Redirect(Url.Action("Index", "Home"));
        }

        public IActionResult Error()
        {
            ViewData["RequestId"] = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            return View();
        }
    }
}
