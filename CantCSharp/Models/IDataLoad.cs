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
        void InsertQuestion(string title, string message, string user);
        void InsertAnswer(string answer, string username, string imageSource, int id, string link);
        void InsertTag(string tag);
        List<Tag> GetTagsList(string tagName);
        void DeleteDataRow(string querryString);

        void UpdateDataRow(string querryString);
        List<IAnswer> GetAnswerList(string queryString);
        List<Comment> GetCommentList(string queryString);
        void InsertAnswerComment(int answerID, string comment);
        void InsertQuestionComment(int questionID, string comment);

    }
}
