using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CantCSharp.Models
{
    public class CsvLoader: IDataLoad
    {
        public List<QuestionModel> questionList;

        public void AddQuestion(string message, string title)
        {
            questionList.Add(new QuestionModel(questionList.Count + 1, message, title));
        }

        public List<QuestionModel> LoadData(string fileroute)
        {
            List<QuestionModel> questions = new List<QuestionModel>();
            using (var reader = new StreamReader(fileroute))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(';');

                    questions.Add(new QuestionModel(Convert.ToInt32(values[0]), values[1], values[2]));
                }
                   
            }
            return questions;
        }
    }
}
