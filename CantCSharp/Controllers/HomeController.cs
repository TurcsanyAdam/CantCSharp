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
        
        private readonly IDataLoad _loader;
        
        public HomeController(ILogger<HomeController> logger, IDataLoad loader)
        {
            
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
        public IActionResult Tags()
        {
            List<Tag> tagList = _loader.GetTagsList("SELECT tag.tag_id,tag.tag_name,COUNT(question_tag.tag_id) FROM tag LEFT JOIN question_tag ON tag.tag_id = question_tag.tag_id GROUP BY(tag.tag_id, tag.tag_name) ORDER BY(tag.tag_id,tag.tag_name)");
            return View(tagList);
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
               [FromForm(Name = "username")] string user, [FromForm(Name = "tag[]")] string[] tags, [FromForm(Name = "newTag")] string newTag)
        {
            string[] newTags = new string[0];
            if (newTag != null )
            {
                newTags = newTag.Split(",");
                foreach (string newtag in newTags)
                {
                    _loader.InsertTag(newtag);
                }

            }


            string[] combinedTags = tags.Concat(newTags).ToArray();

            int questionID = _loader.InsertQuestion(title, message, user);
            foreach(string tag in combinedTags)
            {
                int tagID = _loader.ReturnTagID(tag);
                _loader.InsertQuestionTagRelation(questionID, tagID);
            }

            List<QuestionModel> questionListModel = _loader.GetDataList("SELECT * FROM question;");
            return View("AllQuestions", questionListModel);
        } 

        [HttpPost]
        public void NewAnswer([FromForm(Name = "answer")] string answer, [FromForm(Name = "username")] string username,[FromForm(Name ="Image")] string imagesource,
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

            Response.Redirect($"https://localhost:5001/Home/QuestionDetails/{id}");
        }

        [HttpPost]
        public void DeleteAnswer(int answerID, int questionID)
        {
            _loader.DeleteDataRow($"DELETE FROM answer WHERE answer_id = {answerID}; SELECT * FROM question WHERE question_id = {questionID}");
            Response.Redirect($"QuestionDetails/{questionID}");
        }
        [HttpPost]
        public void DeleteTag(string tagName, int questionID)
        {
            int tagID = _loader.ReturnTagID(tagName);

            _loader.DeleteDataRow($"DELETE FROM question_tag WHERE tag_id = {tagID} AND question_ID = {questionID}; SELECT * FROM question WHERE question_id = {questionID}");
            Response.Redirect($"QuestionDetails/{questionID}");

        }

        [HttpPost]
        public IActionResult EditAnswer(int editAnswerID, int editQuestionID)
        {
            IAnswer answer = _loader.GetAnswerList($"SELECT * FROM answer Where answer_id = {Convert.ToString(editAnswerID)} and question_id = {Convert.ToString(editQuestionID)} ")[0];
            return View("EditAnswer", answer);
        }

        public void EditAnswerConfirm(int editedAnswerID, int editedQuestionID,[FromForm(Name = "EditedAnswer")] string editedAnswer)
        {
            _loader.UpdateDataRow($"Update answer SET answer_message = '{editedAnswer}' Where answer_id = {Convert.ToString(editedAnswerID)} and question_id = {Convert.ToString(editedQuestionID)}");
            Response.Redirect($"QuestionDetails/{editedQuestionID}");
        }

        [HttpPost]
        public IActionResult EditQuestion(int EditID)
        {
            QuestionModel questionModel = _loader.GetDataList($"SELECT * FROM  question WHERE question_id = {Convert.ToString(EditID)}")[0];
            return View("EditQuestion", questionModel);
        }

        [HttpPost]
        public void EditQuestionConfirm(int EditedID ,[FromForm(Name = "EditedMessage")] string editedmMessage )
        {
            _loader.UpdateDataRow($"Update question SET question_message = '{editedmMessage}' WHERE question_id = {Convert.ToString(EditedID)}");
            Response.Redirect($"QuestionDetails/{EditedID}");
        }

        [HttpPost]
        public void VoteUp(int answerID, int questionID)
        {
            _loader.UpdateDataRow($"UPDATE answer SET vote_number = vote_number + 1 WHERE answer_id = {answerID};");
            Response.Redirect($"QuestionDetails/{questionID}");

        }

        [HttpPost]
        public void VoteDown(int answerID, int questionID)
        {
            QuestionModel questionModel = _loader.GetDataList($"UPDATE answer SET vote_number = vote_number - 1 WHERE answer_id = {answerID}; SELECT * FROM question WHERE question_id = {questionID}")[0];

            Response.Redirect($"QuestionDetails/{questionID}");

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

            foreach(QuestionModel question in questionModelList)
            {
                List<IAnswer> resultAnswerList = new List<IAnswer>();
                foreach (Answer answer in question.AnswerList)
                {
                    if (answer.AnswerMessage.ToLower().Contains(searchPattern.ToLower()))
                    {
                        resultAnswerList.Add(answer);
                    }
                }

                question.AnswerList = resultAnswerList;
            }

            questionModelList.Sort((q1, q2) => q2.VoteNumber.CompareTo(q1.VoteNumber));
            SearchDataModel searchDataModel = new SearchDataModel(questionModelList, searchPattern);

            return View("SearchResult", searchDataModel);
        }

        [HttpPost]
        public IActionResult AllQuestionComment(int QuestionCommentID)
        {
            QuestionModel questionModel = _loader.GetDataList($"Select * from question WHERE question_id = {Convert.ToString(QuestionCommentID)}")[0];
            return View("AllComments", questionModel);
        }
        [HttpPost]
        public IActionResult AllAnswerComment(int allCommentAnswerID, int allCommentQuestionId)
        {
            IAnswer answer = _loader.GetAnswerList($"Select * from answer WHERE question_id = {Convert.ToString(allCommentQuestionId)} and answer_id = {Convert.ToString(allCommentAnswerID)}")[0];
            return View("AllAnswerComments", answer);
        }

        [HttpPost]
        public IActionResult WriteQuestionComment(int QuestionCommentID)
        {
            QuestionModel questionModel = _loader.GetDataList($"Select * from question WHERE question_id = {Convert.ToString(QuestionCommentID)}")[0];
            return View("CommentToQuestion", questionModel);
        }

        [HttpPost]
        public IActionResult WriteAnswerComment(int commentQuestionId, int commentAnswerID)
        {
            IAnswer answer = _loader.GetAnswerList($"Select * from answer WHERE question_id = {Convert.ToString(commentQuestionId)} and answer_id = {Convert.ToString(commentAnswerID)}")[0];
            return View("CommentToAnswer", answer);
        }
        [HttpPost]
        public IActionResult PostTheQuestionComment(int NewCommentedQuestionID, [FromForm(Name = "username")] string username, [FromForm(Name = "comment")] string comment)
        {
            _loader.InsertQuestionComment(NewCommentedQuestionID,comment,username);
            QuestionModel questionListModel = _loader.GetDataList($"SELECT * FROM question WHERE question_id = {Convert.ToString(NewCommentedQuestionID)}")[0];
            return View("AllComments", questionListModel);
        }

        [HttpPost]
        public IActionResult PostTheAnswerComment(int NewCommentedAnswerID,int necessaryQuestionID, [FromForm(Name = "username")] string username, [FromForm(Name = "comment")] string comment)
        {
            _loader.InsertAnswerComment(NewCommentedAnswerID,comment,username);
            IAnswer answer = _loader.GetAnswerList($"SELECT * FROM answer WHERE question_id = {Convert.ToString(necessaryQuestionID)} and answer_id = {Convert.ToString(NewCommentedAnswerID)} ")[0];
            return View("AllAnswerComments", answer);
        }

        [HttpPost]
        public IActionResult EditQuestionComment(int editCommentID,int GivenQuestionId)
        {
            Comment comment = _loader.GetCommentList($"SELECT * FROM  askmate_question_comment WHERE comment_id = {Convert.ToString(editCommentID)} and question_id= {Convert.ToString(GivenQuestionId)}")[0];
            return View("EditQuestionComment", comment);
        }

        [HttpPost]
        public IActionResult EditAnswerComment(int editCommentID, int GivenAnswerId)
        {
            Comment comment = _loader.GetCommentList($"SELECT * FROM  askmate_answer_comment WHERE comment_id = {Convert.ToString(editCommentID)} and answer_id= {Convert.ToString(GivenAnswerId)}")[0];
            return View("EditAnswerComment", comment);
        }

        [HttpPost]
        public void ConfirmEditedQuestionComment(int EditedCommentID,int EditedCommentQuestionID, [FromForm(Name = "EditComment")] string editedcomment)
        {
            _loader.UpdateDataRow($"Update askmate_question_comment SET comment_message = '{editedcomment}',edited_number = edited_number + 1 Where comment_id = {Convert.ToString(EditedCommentID)} and question_id = {Convert.ToString(EditedCommentQuestionID)}");
            Response.Redirect($"QuestionDetails/{EditedCommentQuestionID}");
        }

        [HttpPost]
        public void ConfirmEditedAnswerComment(int EditedCommentID, int EditedCommentAnswerID, [FromForm(Name = "EditComment")] string editedcomment)
        {
            IAnswer answer = _loader.GetAnswerList($"Select * From answer Where answer_id = {Convert.ToString(EditedCommentAnswerID)} ")[0];
            _loader.UpdateDataRow($"Update askmate_answer_comment SET comment_message = '{editedcomment}',edited_number = edited_number + 1 Where comment_id = {Convert.ToString(EditedCommentID)} and answer_id = {Convert.ToString(EditedCommentAnswerID)}");
            Response.Redirect($"QuestionDetails/{answer.QuestionID}");
        }

        [HttpPost]
        public void DeleteQuestionComment(int deleteCommentID, int GivenQuestionId)
        {
            
            _loader.DeleteDataRow($"DELETE FROM askmate_question_comment WHERE comment_id = {deleteCommentID} and question_id = {GivenQuestionId}");
            Response.Redirect($"QuestionDetails/{GivenQuestionId}");
        }

        [HttpPost]
        public void DeleteAnswerComment(int deleteCommentID, int GivenAnswerId)
        {
            IAnswer answer = _loader.GetAnswerList($"Select * From answer Where answer_id = {Convert.ToString(GivenAnswerId)} ")[0];
            _loader.DeleteDataRow($"DELETE FROM askmate_answer_comment WHERE comment_id = {deleteCommentID} and answer_id = {GivenAnswerId}");
            Response.Redirect($"QuestionDetails/{answer.QuestionID}");
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
