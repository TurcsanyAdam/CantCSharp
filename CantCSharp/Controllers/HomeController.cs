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
            return View(ListLastFiveQuestions());
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

            return View("Index", ListLastFiveQuestions());
        }

        [HttpGet]
        public IActionResult AskQuestion()
        {
            List <Tag> tagList = _loader.GetTagsList("SELECT * FROM tag");
            return View("AskQuestion",tagList);
        }

        [HttpPost]
        public IActionResult NewQuestion([FromForm(Name = "title")] string title, [FromForm(Name = "message")] string message, 
               [FromForm(Name = "username")] string user, [FromForm(Name = "tag[]")] string[] tags)
        {
            _loader.InsertQuestion(title, message, user);

            List<QuestionModel> questionListModel = _loader.GetDataList("SELECT * FROM question;");
            return View("AllQuestions", questionListModel);
        } 

        [HttpPost]
        public IActionResult NewAnswer([FromForm(Name = "answer")] string answer, [FromForm(Name = "username")] string username,[FromForm(Name ="Image")] string imagesource,
           int id, [FromForm(Name = "Link")]string link)
        {
            if(imagesource != null)
            {
                _loader.InsertAnswer(answer, username, imagesource, id, link);
            }
            else
            {
                _loader.InsertAnswer(answer, username, "", id, link);

            }

            QuestionModel questionModel = _loader.GetDataList($"SELECT * FROM question WHERE question_id = {id};")[0];
            return View("QuestionDetails", questionModel);
        }

        [HttpPost]
        public IActionResult DeleteAnswer(int answerID, int questionID)
        {
            QuestionModel questionModel = _loader.GetDataList($"DELETE FROM answer WHERE answer_id = {answerID}; SELECT * FROM question WHERE question_id = {questionID}")[0];

            return View("QuestionDetails", questionModel);
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

        
        public IActionResult SortByAnswersCount()
        {
            var questionModel = _loader.GetDataList("Select question.question_id, question.submission_time, view_number, question.vote_number, question_title, question_message, question_image from question left join answer on question.question_id = answer.question_id " +
                                                    "group by question.question_id " +
                                                    "Order by count(answer_id) desc");
            
            return View("AllQuestions", questionModel);
        }

        [HttpGet]
        public IActionResult Search(string searchPattern)
        {
            List<QuestionModel> questionModelList = _loader.GetDataList("SELECT q.* FROM question q " +
                                                "LEFT JOIN answer a ON q.question_id = a.question_id " +
                                                $"WHERE q.question_title ILIKE '%{searchPattern}%' " +
                                                $"OR q.question_message ILIKE '%{searchPattern}%' " +
                                                $"OR a.answer_message ILIKE '%{searchPattern}%' " +
                                                "GROUP BY q.question_id");

            SortedDictionary<QuestionModel, List<Answer>> resultDict = new SortedDictionary<QuestionModel, List<Answer>>();
            foreach (QuestionModel question in questionModelList)
            {
                resultDict.Add(question, new List<Answer>());
                foreach (Answer answer in question.AnswerList)
                {
                    if (answer.AnswerMessage.ToLower().Contains(searchPattern.ToLower()))
                    {
                        resultDict[question].Add(answer);
                    }
                }
            }

            SearchDataModel searchDataModel = new SearchDataModel(resultDict, searchPattern);

            return View("SearchResult", searchDataModel);
        }

        [HttpPost]
        public IActionResult AllQuestionComment(int QuestionCommentID)
        {
            QuestionModel questionModel = _loader.GetDataList($"Select * from question WHERE question_id = {Convert.ToString(QuestionCommentID)}")[0];
            return View("AllComments", questionModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private List<QuestionModel> ListLastFiveQuestions()
        {
            return _loader.GetDataList("SELECT * FROM question ORDER BY submission_time DESC LIMIT 5;");
        }
    }
}
