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
            List<QuestionModel> questionListModel = _loader.GetDataList("SELECT * FROM question ORDER BY submission_time LIMIT 5;");
            return View(questionListModel);
        }

        public IActionResult AllQuestions()
        {
            List<QuestionModel> questionListModel = _loader.GetDataList("SELECT * FROM question;");
            return View(questionListModel);
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
            QuestionModel questionModel = _loader.GetDataList($"SELECT * FROM question WHERE question_id = {id};")[0];
            questionModel.ViewNumber++;
            return View(questionModel);
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

        public IActionResult EditAnswer(int editAnswerID, int editQuestionID)
        {
            foreach (QuestionModel question in _loader.QuestionList)
            {
                if (question.QuestionID == editQuestionID)
                {
                    foreach (Answer answer in question.AnswerList)
                    {
                        if (answer.Id == editAnswerID)
                        {
                            return View("EditAnswer", answer);
                            
                        }
                    }
                }
                
            }
            return View("Index", _loader.QuestionList);
        }

        public IActionResult EditAnswerConfirm(int editedAnswerID, int editedQuestionID,[FromForm(Name = "EditedAnswer")] string editedAnswer)
        {
            foreach (QuestionModel question in _loader.QuestionList)
            {
                if (question.QuestionID == editedQuestionID)
                {
                    foreach (Answer answer in question.AnswerList)
                    {
                        if (answer.Id == editedAnswerID)
                        {
                            answer.AnswerMessage = editedAnswer;

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

        public IActionResult SortByTitle()
        {
            List<QuestionModel> questionModelList = _loader.GetDataList("SELECT * FROM question ORDER BY question_title ASC;");
            return View("AllQuestions", questionModelList);
        }

        public IActionResult SortByVotes()
        {
            List<QuestionModel> questionModelList = _loader.GetDataList("SELECT * FROM question ORDER BY vote_number DESC;");
            return View("AllQuestions", questionModelList);
        }

        public IActionResult SortByDate()
        {
            List<QuestionModel> questionModelList = _loader.GetDataList("SELECT * FROM question ORDER BY submission_time DESC;");
            return View("AllQuestions", questionModelList);
        }

        public IActionResult SortByViews()
        {
            List<QuestionModel> questionModelList = _loader.GetDataList("SELECT * FROM question ORDER BY view_number DESC;");
            return View("AllQuestions", questionModelList);
        }

        // NOT DONE!
        public IActionResult SortByAnswersCount()
        {
            var questionModel = _loader.QuestionList;
            questionModel.Sort((q1, q2) => q2.AnswerList.Count.CompareTo(q1.AnswerList.Count));
            return View("AllQuestions", questionModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
