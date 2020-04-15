using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CantCSharp.Models
{
    public interface IDataLoad
    {
        List<QuestionModel> QuestionList { get; set; }
        List<QuestionModel> GetDataList(string queryString);
        int CheckIfUserExists(string username, string password);
        void InsertUser(string username, string password, string email);
        void ModifyReputation(string answerOrQuestion,int id, int reputation);

        int InsertQuestion(string title, string message, string user, int UserID,bool IsAnswered);
        void InsertAnswer(string answer, string username, string imageSource, int id, string link, int UserID,bool IsSolution);
        void InsertTag(string tag);
        void InsertQuestionTagRelation(int question_ID, int tag_ID);
        int ReturnTagID(string tag_name);

        List<Tag> GetTagsList(string tagName);
        void DeleteDataRow(string querryString);

        void UpdateDataRow(string querryString);
        List<User> GetUserList(string queryString);
        List<IAnswer> GetAnswerList(string queryString);
        List<Comment> GetCommentList(string queryString);
        void InsertAnswerComment(int answerID, string comment,string username, int UserID);
        void InsertQuestionComment(int questionID, string comment,string username, int UserID);

    }
}
