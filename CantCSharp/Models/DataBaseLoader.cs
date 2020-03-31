using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;

namespace CantCSharp.Models
{
    public class DataBaseLoader : IDataLoad
    {
        private static readonly string dbHost = Environment.GetEnvironmentVariable("DB_HOST");
        private static readonly string dbUser = Environment.GetEnvironmentVariable("DB_USER");
        private static readonly string dbPass = Environment.GetEnvironmentVariable("DB_PASS");
        private static readonly string dbName = Environment.GetEnvironmentVariable("DB_NAME");
        public static readonly string connectingString = $"Host={dbHost};Username={dbUser};Password={dbPass};Database={dbName}";
        public List<QuestionModel> QuestionList { get; set; } = new List<QuestionModel>();
        public List<IAnswer> AnswerList { get; set; } = new List<IAnswer>();

       
        public void InsertQuestion(string title, string message, string user)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectingString))
            {
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand($"INSERT INTO question(submission_time, view_number, vote_number, question_title, question_message, question_image)" +
                    $"VALUES ((@time), 0, 0, (@title), (@message), null)", connection);
                command.Parameters.AddWithValue("time", DateTime.Now);
                command.Parameters.AddWithValue("title", title);
                command.Parameters.AddWithValue("message", message);
                command.ExecuteNonQuery();
            }
        }

        public void InsertAnswer(string answer, string username, string imageSource,int id, string link)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectingString))
            {
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand($"INSERT INTO answer(submission_time, vote_number, question_id, answer_message, answer_image)" +
                    $"VALUES ((@time), 0, (@id), (@answer), (@answer_image))", connection);
                command.Parameters.AddWithValue("time", DateTime.Now);
                command.Parameters.AddWithValue("id", id);
                command.Parameters.AddWithValue("answer", answer);
                command.Parameters.AddWithValue("answer_image", imageSource);
                command.ExecuteNonQuery();
            }
        }

        public List<QuestionModel> GetDataList(string queryString)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectingString))
            {
                QuestionList.Clear();
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand(queryString, connection);
                NpgsqlDataReader dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    QuestionModel question = new QuestionModel(Convert.ToInt32(dataReader[0]),
                                                       DateTime.Parse(dataReader[1].ToString()),
                                                       Convert.ToInt32(dataReader[2]),
                                                       Convert.ToInt32(dataReader[3]),
                                                       dataReader[4].ToString(),
                                                       dataReader[5].ToString(),
                                                       "TestUser");
                    AnswerList = GetAnswerList($"SELECT * FROM answer WHERE question_id = {question.QuestionID} ORDER BY vote_number DESC");
                    question.AnswerList = AnswerList;
                    QuestionList.Add(question);
                }
            }

            return QuestionList;
        }
        public List<IAnswer> GetAnswerList(string queryString)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectingString))
            {
                AnswerList.Clear();
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand(queryString, connection);
                NpgsqlDataReader dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    AnswerList.Add(new Answer("TestUser",
                                                       Convert.ToInt32(dataReader[0]),
                                                       DateTime.Parse(dataReader[1].ToString()),
                                                       Convert.ToInt32(dataReader[2]),
                                                       Convert.ToInt32(dataReader[3]),
                                                       dataReader[4].ToString(),
                                                       "https://upload.wikimedia.org/wikipedia/commons/thumb/5/54/American_Broadcasting_Company_Logo.svg/1200px-American_Broadcasting_Company_Logo.svg.png",
                                                       "www.google.com",
                                                       false));
                }
            }

            return AnswerList;
        }
        public void DeleteDataRow(string queryString)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectingString))
            {
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand(queryString, connection);
                NpgsqlDataReader dataReader = command.ExecuteReader();
            }
        }

        public void UpdateDataRow(string queryString)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectingString))
            {
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand(queryString, connection);
                NpgsqlDataReader dataReader = command.ExecuteReader();
            }

        }
    }
}
