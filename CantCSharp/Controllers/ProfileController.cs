using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CantCSharp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CantCSharp.Controllers
{
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