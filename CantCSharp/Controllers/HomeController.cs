﻿using System;
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
            question.ViewNumber++;

            return View(question);
        }

        [HttpGet]
        public IActionResult AskQuestion()
        {
            return View("AskQuestion");
        }

       [HttpPost]
        public IActionResult NewQuestion([FromForm(Name = "title")] string title, [FromForm(Name = "message")] string message, 
               [FromForm(Name = "username")] string user)
        {
            _loader.AddQuestion(title, message, user);
            return View("Index",_loader.QuestionList);
        }

        [HttpPost("QuestionDetails/{id}/NewAnswer")]
        public IActionResult NewAnswer(int id,[FromForm(Name = "answer")] string answer, [FromForm(Name = "username")] string username)
        {
            var question = _loader.QuestionList.FirstOrDefault(q => q.QuestionID == id);
            Answer newAnswer = new Answer(question.AnswerList.Count+1, username, answer);
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
