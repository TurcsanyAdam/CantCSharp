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
        private static List<QuestionModel> questionModel;
        public HomeController(ILogger<HomeController> logger, IDataLoad loader)
        {
            _logger = logger;
            _loader = loader;
            questionModel = _loader.LoadData("wwwroot/csv/questions.csv");

        }
        public IActionResult Index()
        {
            
            return View(questionModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult QuestionDetails()
        {
            return View();
        }

        public IActionResult NewQuestion()
        {
            return View();
        }

       [HttpPost]
        public IActionResult NewQuestion([FromForm(Name = "Title")] string title, [FromForm(Name = "message")] string message)
        {
            questionModel.Add(new QuestionModel(345, message, title));
            return View(questionModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
