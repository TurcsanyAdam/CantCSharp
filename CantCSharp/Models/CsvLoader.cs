using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CantCSharp.Models
{
    public class CsvLoader: IDataLoad
    {
        public List<QuestionModel> QuestionList { get; set; } = new List<QuestionModel>();

        public CsvLoader()
        {
            LoadData("wwwroot/csv/questions.csv","something");
            LoadAnswer("wwwroot/csv/answers.csv");

            SetupSortTest();
        }
        public void AddQuestion(string message, string title, string user)
        {
            QuestionList.Add(new QuestionModel(QuestionList.Count + 1, message, title, user));
        }

        public List<QuestionModel> LoadData(string fileroute,string justsomething)
        {
            
            using (var reader = new StreamReader(fileroute))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(';');

                    QuestionList.Add(new QuestionModel(values[0], Convert.ToInt32(values[1]), values[2], Convert.ToInt32(values[3]), Convert.ToInt32(values[4]), values[5], values[6], Convert.ToBoolean(values[7]), Convert.ToBoolean(values[8])));
                }
                   
            }
            return QuestionList;
        }
        public List<QuestionModel> LoadAnswer(string fileroute)
        {

            using (var reader = new StreamReader(fileroute))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(';');

                    Answer answer = new Answer(values[0], Convert.ToInt32(values[1]), Convert.ToDateTime(values[2]), Convert.ToInt32(values[3]), Convert.ToInt32(values[4]), values[5], values[6], values[7], Convert.ToBoolean(values[8]));
                    foreach(QuestionModel question in QuestionList)
                    {
                        if(answer.QuestionID == question.QuestionID)
                        {
                            question.AnswerList.Add(answer);
                        }
                    }
                }

            }
            return QuestionList;
        }

        public void SetupSortTest()
        {
            QuestionList[0].PostTime = QuestionList[0].PostTime.AddDays(3.0);
            QuestionList[2].PostTime = QuestionList[0].PostTime.AddDays(5.0);
        }
    }
}
