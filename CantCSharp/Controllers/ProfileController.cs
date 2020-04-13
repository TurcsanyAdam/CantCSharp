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

        public IActionResult Index()
        {
            return View();
        }

        
    }
}