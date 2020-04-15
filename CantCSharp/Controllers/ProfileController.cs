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
            var user = HttpContext.User;
            var claim = user.Claims.First(c => c.Type == ClaimTypes.Email);
            var email = claim.Value;
            User searchedUser = _loader.GetUserList($"Select * FROM users WHERE email = '{email}'")[0];

            

            return View(searchedUser);
        }
    }
}