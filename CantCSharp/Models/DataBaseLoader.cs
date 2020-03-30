using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;

namespace CantCSharp.Models
{
    public class DataBaseLoader : IDataLoad
    {
        private static readonly string dbHost = "localhost";//Environment.GetEnvironmentVariable("DB_HOST");
        private static readonly string dbUser = "postgres";//Environment.GetEnvironmentVariable("DB_USER");
        private static readonly string dbPass = "admin";//Environment.GetEnvironmentVariable("DB_PASS");
        private static readonly string dbName = "askmate";//Environment.GetEnvironmentVariable("DB_NAME");
        public static readonly string connectingString = $"Host={dbHost};Username={dbUser};Password={dbPass};Database={dbName}";
        public List<QuestionModel> QuestionList { get; set; } = new List<QuestionModel>();

        public DataBaseLoader()
        {
            
        }
        public void AddQuestion(string message, string title, string user)
        {
            //QuestionList.Add(new QuestionModel(QuestionList.Count + 1, message, title, user));
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
                    QuestionList.Add(new QuestionModel(Convert.ToInt32(dataReader[0]),
                                                       DateTime.Parse(dataReader[1].ToString()),
                                                       Convert.ToInt32(dataReader[2]),
                                                       Convert.ToInt32(dataReader[3]),
                                                       dataReader[4].ToString(),
                                                       dataReader[5].ToString(),
                                                       "TestUser"));
                }
            }

            return QuestionList;
        }
    }
}
