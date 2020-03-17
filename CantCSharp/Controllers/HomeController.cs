using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CantCSharp.Models;

namespace CantCSharp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDataLoad _loader;
        
        public HomeController(ILogger<HomeController> logger, IDataLoad loader)
        {
            _logger = logger;
            _loader = loader;
            


        }
        public IActionResult Index()
        {
            
            return View(_loader.QuestionList);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        [HttpGet]
        public IActionResult QuestionDetails(int id)
        {
            var questionModel = _loader.QuestionList;
            var question = questionModel.FirstOrDefault(q => q.QuestionID == id);
            return View(question);
        }

        public IActionResult NewQuestion()
        {
            return View();
        }

       [HttpPost]
        public IActionResult NewQuestion([FromForm(Name = "Title")] string title, [FromForm(Name = "message")] string message)
        {
            _loader.AddQuestion(title,message);
            return View("Index",_loader.QuestionList);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
