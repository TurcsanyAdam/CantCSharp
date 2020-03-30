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
            var topFiveQuestions = questionModel.OrderByDescending(q => q.PostTime).Take(5).ToList();

            return View(topFiveQuestions);
        }

        public IActionResult AllQuestions()
        {
            var questionModel = _loader.QuestionList;
            questionModel.Sort((q1, q2) => q1.PostTime.CompareTo(q2.PostTime));
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

        [HttpPost]
        [ActionName("QuestionDetails")]
        public IActionResult DeleteQuestion([FromForm(Name = "TheId")] int ID)
        {
            foreach(QuestionModel question in _loader.QuestionList)
            {
                if(question.QuestionID == ID)
                {
                    _loader.QuestionList.Remove(question);
                    break;
                }
            }
            return View("Index", _loader.QuestionList);
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
            return View("Index", _loader.QuestionList);
        } 

        [HttpPost]
        public IActionResult NewAnswer([FromForm(Name = "answer")] string answer, [FromForm(Name = "username")] string username,[FromForm(Name ="Image")] string imagesource,
           int id, [FromForm(Name = "Link")]string link)
        {
            var question = _loader.QuestionList.FirstOrDefault(q => q.QuestionID == id);
            if(imagesource== null)
            {
                imagesource = "https://mytrendingstories.com/media/photologue/photos/cache/keep-calm-and-git-gud-10_article_large.png";
            }
            Answer newAnswer = new Answer(question.AnswerList.Count+1, username, answer, imagesource,id);
            if (link != null)
            {
                newAnswer.Link = link.Split(' ');
            }
            question.AnswerList.Add(newAnswer);
            return View(_loader.QuestionList);
        }
        [HttpPost]
        public IActionResult DeleteAnswer(int answerID, int questionID)
        {
            foreach(QuestionModel question in _loader.QuestionList)
            {
               if(question.QuestionID == questionID)
                {
                    foreach(Answer answer in question.AnswerList)
                    {
                        if(answer.Id == answerID)
                        {
                            question.AnswerList.Remove(answer);
                            break;
                        }
                    }
                }
            }

            return View("Index", _loader.QuestionList);
        }
        [HttpPost]
        
        public IActionResult EditQuestion(int EditID)
        {
            
            QuestionModel questionModel = _loader.QuestionList.Where(q => q.QuestionID == EditID).FirstOrDefault();
            return View("EditQuestion", questionModel);

        }
        [HttpPost]
        
        public IActionResult EditQuestionConfirm(int EditedID ,[FromForm(Name = "EditedMessage")] string editedmessage )
        {
            QuestionModel questionModel = _loader.QuestionList.Where(q => q.QuestionID == EditedID).FirstOrDefault();
            questionModel.QuestionMessage = editedmessage;
            return View("Index", _loader.QuestionList);
        }
        [HttpPost]
        public IActionResult VoteUp(int answerID, int questionID)
        {
            QuestionModel questionModel = _loader.QuestionList.Where(q => q.QuestionID == questionID).FirstOrDefault();

            foreach (QuestionModel question in _loader.QuestionList)
            {
                if (question.QuestionID == questionID)
                {
                    foreach (Answer answer in question.AnswerList)
                    {
                        if (answer.Id == answerID)
                        {
                            answer.VoteNumber++;
                            break;
                        }
                    }
                }
            }
            questionModel.AnswerList.Sort((x, y) => y.VoteNumber.CompareTo(x.VoteNumber));

            return View("QuestionDetails", questionModel);


        }

        [HttpPost]
        public IActionResult VoteDown(int answerID, int questionID)
        {
            QuestionModel questionModel = _loader.QuestionList.Where(q => q.QuestionID == questionID).FirstOrDefault();

            foreach (QuestionModel question in _loader.QuestionList)
            {
                if (question.QuestionID == questionID)
                {
                    foreach (Answer answer in question.AnswerList)
                    {
                        if (answer.Id == answerID)
                        {
                            answer.VoteNumber -= 1;
                            break;
                        }
                    }
                }
            }
            questionModel.AnswerList.Sort((x, y) => y.VoteNumber.CompareTo(x.VoteNumber));

            return View("QuestionDetails", questionModel);

        }

        public IActionResult SortByDate()
        {
            var questionModel = _loader.QuestionList;
            questionModel.Sort((q1, q2) => q2.PostTime.CompareTo(q1.PostTime));
            return View("Index", questionModel);
        }

        public IActionResult SortByViews()
        {
            var questionModel = _loader.QuestionList;
            questionModel.Sort((q1, q2) => q2.ViewNumber.CompareTo(q1.ViewNumber));
            return View("Index", questionModel);
        }

        public IActionResult SortByAnswersCount()
        {
            var questionModel = _loader.QuestionList;
            questionModel.Sort((q1, q2) => q2.AnswerList.Count.CompareTo(q1.AnswerList.Count));
            return View("Index", questionModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
