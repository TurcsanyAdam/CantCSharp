﻿using System;
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
        public void DeleteDataRow(string queryString)
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
