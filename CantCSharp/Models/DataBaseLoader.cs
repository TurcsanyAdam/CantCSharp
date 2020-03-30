using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;

namespace CantCSharp.Models
{
    //public class DataBaseLoader : IDataLoad
    //{
    //    public List<QuestionModel> QuestionList { get; set; } = new List<QuestionModel>();

    //    public void AddQuestion(string message, string title, string user)
    //    {
    //        QuestionList.Add(new QuestionModel(QuestionList.Count + 1, message, title, user));
    //    }

    //    List<QuestionModel> LoadData(string connectingString,string select)
    //    { 
    //        using(NpgsqlConnection connection = new NpgsqlConnection(connectingString))
    //        {
    //            connection.Open();
    //            NpgsqlCommand command = new NpgsqlCommand(select, connection);
    //            NpgsqlDataReader datareader = command.ExecuteReader();

    //            while (datareader.Read())
    //            {

    //            }

    //        }
    //    }


    //}
}
