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
            _loader.DeleteDataRow($"DELETE FROM question WHERE question_id = {Convert.ToString(ID)}");
            var questionModel = _loader.GetDataList("SELECT * FROM question;");

            return View("Index", questionModel);
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
            _loader.InsertQuestion(title, message, user);

            List<QuestionModel> questionListModel = _loader.GetDataList("SELECT * FROM question;");
            return View("AllQuestions", questionListModel);
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
            IAnswer answer = _loader.GetAnswerList($"SELECT * FROM answer Where answer_id = {Convert.ToString(editAnswerID)} and question_id = {Convert.ToString(editQuestionID)} ")[0];
            return View("EditAnswer", answer);
              

            
        }

        public IActionResult EditAnswerConfirm(int editedAnswerID, int editedQuestionID,[FromForm(Name = "EditedAnswer")] string editedAnswer)
        {
            _loader.UpdateDataRow($"Update answer SET answer_message = '{editedAnswer}' Where answer_id = {Convert.ToString(editedAnswerID)} and question_id = {Convert.ToString(editedQuestionID)}");
            var questionModel = _loader.GetDataList("SELECT * FROM question;");
            return View("Index", questionModel);
        }
    
        [HttpPost]
        public IActionResult EditQuestion(int EditID)
        {

            QuestionModel questionModel = _loader.GetDataList($"SELECT * FROM  question WHERE question_id = {Convert.ToString(EditID)}")[0];
            return View("EditQuestion", questionModel);

        }

        [HttpPost]
        public IActionResult EditQuestionConfirm(int EditedID ,[FromForm(Name = "EditedMessage")] string editedmMessage )
        {
            _loader.UpdateDataRow($"Update question SET question_message = '{editedmMessage}' WHERE question_id = {Convert.ToString(EditedID)}");
            var questionModel = _loader.GetDataList("SELECT * FROM question;");
            return View("Index", questionModel);
        }

        [HttpPost]
        public IActionResult VoteUp(int answerID, int questionID)
        {
            QuestionModel questionModel = _loader.GetDataList($"UPDATE answer SET vote_number = vote_number + 1 WHERE answer_id = {answerID}; SELECT * FROM question WHERE question_id = {questionID}")[0];

            return View("QuestionDetails",questionModel);


        }

        [HttpPost]
        public IActionResult VoteDown(int answerID, int questionID)
        {
            QuestionModel questionModel = _loader.GetDataList($"UPDATE answer SET vote_number = vote_number - 1 WHERE answer_id = {answerID}; SELECT * FROM question WHERE question_id = {questionID}")[0];

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

        [HttpGet]
        public IActionResult Search(string searchPattern)
        {
            List<QuestionModel> questionModelList = _loader.GetDataList($"SELECT * FROM question WHERE question_title ILIKE '%{searchPattern}%' OR question_message ILIKE '%{searchPattern}%';");
            return View("SearchResult", questionModelList);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
