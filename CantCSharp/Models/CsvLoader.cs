using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CantCSharp.Models
{
    public class CsvLoader
    {
        public static List<QuestionModel> LoadData()
        {
            List<QuestionModel> questions = new List<QuestionModel>();
            using (var reader = new StreamReader(@"C:\Users\Turi\source\repos\CantCSharp\CantCSharp\wwwroot\csv\questions.csv"))
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
