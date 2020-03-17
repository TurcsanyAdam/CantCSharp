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
            var questionModel = _loader.QuestionList;
            questionModel[0].MarkAsAnswered();
            questionModel[0].ViewNumber++;
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

        [HttpGet]
        public IActionResult QuestionDetails(int id)
        {
            var questionModel = _loader.QuestionList;
            var question = questionModel.FirstOrDefault(q => q.QuestionID == id);
            return View(question);
        }

        [HttpGet]
        public IActionResult AskQuestion()
        {
            return View("AskQuestion");
        }

       [HttpPost]
        public IActionResult NewQuestion([FromForm(Name = "Title")] string title, [FromForm(Name = "message")] string message, string user="noname")
        {
            _loader.AddQuestion(title, message, user);
            return View("Index",_loader.QuestionList);
        }

        [HttpGet]
        public IActionResult NewAnswer([FromForm(Name = "Answer")] string answer)
        {
            Answer newAnswer = new Answer(1, answer);
            var question = _loader.QuestionList.FirstOrDefault(q => q.QuestionID == newAnswer.Id);
            question.AnswerList.Add(newAnswer);

            return View(_loader.QuestionList);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
