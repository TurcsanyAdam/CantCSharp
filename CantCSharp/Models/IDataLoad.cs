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


        void DeleteDataRow(string querryString);

        void UpdateDataRow(string querryString);
    }
}
