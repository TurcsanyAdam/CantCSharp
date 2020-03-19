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
            LoadData("wwwroot/csv/questions.csv");
            SetupSortTest();
        }
        public void AddQuestion(string message, string title, string user)
        {
            QuestionList.Add(new QuestionModel(QuestionList.Count + 1, message, title, user));
        }

        public List<QuestionModel> LoadData(string fileroute)
        {
            
            using (var reader = new StreamReader(fileroute))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(';');

                    QuestionList.Add(new QuestionModel(Convert.ToInt32(values[0]), values[1], values[2], values[3]));
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
