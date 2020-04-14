using System;
using System.Collections.Generic;
using System.Linq;
using CantCSharp.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CantCSharp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IDataLoad _loader;

        public AccountController(IDataLoad loader)
        {
            _loader = loader;
        }
        public IActionResult Login()
        {
            return View();
          
        }
        public IActionResult Registration()
        {
            return View();

        }
        public IActionResult RegistrationComplete([FromForm]string username, [FromForm]string email, [FromForm] string password)
        {
            _loader.InsertUser(username, email, Utility.Hash(password));
            return View();

        }

        [HttpPost]

        public async Task<ActionResult> LoginAsync([FromForm]string email, [FromForm] string password)
        {
            if("test@test.com" != email || "test" != password)
            {
                return RedirectToAction("Login", "Account");
            }
            User user = new User(email, password);
            //here comes the Loginformation storing sql querry;
            var claims = new List<Claim> { new Claim(ClaimTypes.Email, email) };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                //AllowRefresh = <bool>,
                // Refreshing the authentication session should be allowed.

                //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                // The time at which the authentication ticket expires. A 
                // value set here overrides the ExpireTimeSpan option of 
                // CookieAuthenticationOptions set with AddCookie.

                //IsPersistent = true,
                // Whether the authentication session is persisted across 
                // multiple requests. When used with cookies, controls
                // whether the cookie's lifetime is absolute (matching the
                // lifetime of the authentication ticket) or session-based.

                //IssuedUtc = <DateTimeOffset>,
                // The time at which the authentication ticket was issued.

                //RedirectUri = <string>
                // The full path or absolute URI to be used as an http 
                // redirect response value.
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return RedirectToAction("Index", "Profile");
        }
    }
}