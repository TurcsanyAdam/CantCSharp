using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CantCSharp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CantCSharp.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        
        private readonly IDataLoad _loader;
        public ProfileController(IDataLoad loader)
        {
            _loader = loader;
        }

        public IActionResult ProfileDetails()
        {
            User user;
            var username = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.Email).Value;
            User searchedUser = _loader.GetUserList($"Select * FROM users WHERE username = '{username}'")[0];

            user = new User(searchedUser.UserName, searchedUser.Email, searchedUser.Password);

            return View(user);
        }
    }
}