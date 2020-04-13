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

       
        public int InsertQuestion(string title, string message, string question_username)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectingString))
            {
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand($"INSERT INTO question(submission_time, view_number, vote_number, question_title, question_message, question_image, question_username)" +
                    $"VALUES ((@time), 0, 0, (@title), (@message), null, (@question_username)) RETURNING question_id", connection);
                command.Parameters.AddWithValue("time", Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                command.Parameters.AddWithValue("title", title);
                command.Parameters.AddWithValue("message", message);
                command.Parameters.AddWithValue("question_username", question_username);
                int question_ID = Convert.ToInt32(command.ExecuteScalar());

                return question_ID;

            }
        }

        public void InsertAnswer(string answer, string answer_username, string imageSource,int id, string link)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectingString))
            {
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand($"INSERT INTO answer(submission_time, vote_number, question_id, answer_message, answer_image, answer_username)" +
                    $"VALUES ((@time), 0, (@id), (@answer), (@answer_image), (@answer_username))", connection);
                command.Parameters.AddWithValue("time", Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                command.Parameters.AddWithValue("id", id);
                command.Parameters.AddWithValue("answer", answer);
                command.Parameters.AddWithValue("answer_image", imageSource);
                command.Parameters.AddWithValue("answer_username", answer_username);
                command.ExecuteNonQuery();
            }
        }

        public void InsertTag(string tag)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectingString))
            {
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand($"INSERT INTO tag(tag_name) VALUES ((@tag_name)) ON CONFLICT (tag_name) DO NOTHING", connection);
                command.Parameters.AddWithValue("tag_name", tag);
                command.ExecuteNonQuery();
            }
        }
        public void InsertQuestionComment(int questionID,string comment,string username)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectingString))
            {
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand($"INSERT INTO askmate_question_comment(question_id,comment_message,submission_time,edited_number,comment_username)" +
                    $"VALUES ((@id),(@comment),(@time),0,(@username))",connection);
                command.Parameters.AddWithValue("id", questionID);
                command.Parameters.AddWithValue("comment", comment);
                command.Parameters.AddWithValue("time", Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                command.Parameters.AddWithValue("username", username);
                command.ExecuteNonQuery();

            }
        }
        public void InsertAnswerComment(int answerID, string comment, string username)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectingString))
            {
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand($"INSERT INTO askmate_answer_comment(answer_id,comment_message,submission_time,edited_number,comment_username)" +
                    $"VALUES ((@id),(@comment),(@time),0,(@username))", connection);
                command.Parameters.AddWithValue("id", answerID);
                command.Parameters.AddWithValue("comment", comment);
                command.Parameters.AddWithValue("time", Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                command.Parameters.AddWithValue("username", username);
                command.ExecuteNonQuery();

            }
        }
        public void InsertQuestionTagRelation(int question_ID, int tag_ID)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectingString))
            {
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand($"INSERT INTO question_tag(question_id, tag_id) VALUES((@question_ID), (@tag_ID))", connection);
                command.Parameters.AddWithValue("question_ID", question_ID);
                command.Parameters.AddWithValue("tag_ID", tag_ID);
                command.ExecuteNonQuery();

            }
        }
        public int ReturnTagID(string tag_name)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectingString))
            {
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand($"SELECT tag_ID FROM tag WHERE tag_name = '{tag_name}'", connection);
                NpgsqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    return Convert.ToInt32(dataReader[0]);

                }
            }
            return 0;
        }

        public List<Tag> GetTagsList(string queryString)
        {
            List<Tag> tagList = new List<Tag>();

            using (NpgsqlConnection connection = new NpgsqlConnection(connectingString))
            {
                tagList.Clear();
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand(queryString, connection);
                NpgsqlDataReader dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    Tag tag = new Tag(dataReader[1].ToString());
                    if(dataReader.FieldCount >2)
                    {
                        tag.TimesUsedInQuestions = Convert.ToInt32(dataReader[2]);
                    }
                    tagList.Add(tag);
                }
            }

            return tagList;
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
                    QuestionModel question = new QuestionModel(questionid:Convert.ToInt32(dataReader[0]),
                                                       date:DateTime.Parse(dataReader[1].ToString()),
                                                       viewNum:Convert.ToInt32(dataReader[2]),
                                                       questionTitle:dataReader[4].ToString(),
                                                       questionMessage:dataReader[5].ToString(),
                                                       user: dataReader[7].ToString());
                    question.AnswerList = GetAnswerList($"SELECT * FROM answer WHERE question_id = {question.QuestionID} ORDER BY vote_number DESC");
                    question.QuestionComments = GetCommentList($"SELECT * FROM askmate_question_comment WHERE question_id = {question.QuestionID} ");
                    question.TagList = GetTagsList($"SELECT * FROM tag LEFT JOIN question_tag ON tag.tag_id = question_tag.tag_id WHERE question_id = {question.QuestionID} ");
                    question.CalculateUpvotes();
                    QuestionList.Add(question);
                }
            }

            return QuestionList;
        }
        public List<IAnswer> GetAnswerList(string queryString)
        {
            List<IAnswer> AnswerList  = new List<IAnswer>();

            using (NpgsqlConnection connection = new NpgsqlConnection(connectingString))
            {
                
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand(queryString, connection);
                NpgsqlDataReader dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    List<Comment> comments = new List<Comment>();
                   
                   Answer answer = new Answer(dataReader[6].ToString(),
                                                       Convert.ToInt32(dataReader[0]),
                                                       DateTime.Parse(dataReader[1].ToString()),
                                                       Convert.ToInt32(dataReader[2]),
                                                       Convert.ToInt32(dataReader[3]),
                                                       dataReader[4].ToString(),
                                                       "https://upload.wikimedia.org/wikipedia/commons/thumb/5/54/American_Broadcasting_Company_Logo.svg/1200px-American_Broadcasting_Company_Logo.svg.png",
                                                       "www.google.com",
                                                       false);

                   answer.AnswerComments = GetCommentList($"SELECT * FROM askmate_answer_comment WHERE answer_id = {answer.Id} ");
                   AnswerList.Add(answer);
                    
                }
            }

            return AnswerList;
        }
        public List<Comment> GetCommentList(string queryString)
        {
            List<Comment> commentList = new List<Comment>();
            using (NpgsqlConnection connection = new NpgsqlConnection(connectingString))
            {

                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand(queryString, connection);
                NpgsqlDataReader dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    commentList.Add (new Comment(dataReader[2].ToString(),
                                                       DateTime.Parse(dataReader[3].ToString()),
                                                       Convert.ToInt32(dataReader[4]),
                                                       dataReader[5].ToString(),
                                                       Convert.ToInt32(dataReader[0]),
                                                       Convert.ToInt32(dataReader[1])));
                 
                }
            }
            return commentList;

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
