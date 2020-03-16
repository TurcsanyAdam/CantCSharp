using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace CantCSharp.Models
{
    public class QuestionModel : IQuestion
    {
        public int QuestionID { get; }
        public string Question { get; set; }
        public List <IAnswer> Answer { get; set; }
        public DateTime PostTime { get; set; }
        public bool Answered { get; private set; }
        public bool IsClosed { get; private set; }

        public QuestionModel(int questionid,string question)
        {
            QuestionID = questionid;
            Question = question;
            Answer = new List<IAnswer>();
            PostTime = DateTime.Now;
            Answered = false;
            IsClosed = false;
        }

        public void AddAnswer(IAnswer answer)
        {
            Answer.Add(answer);
        }
        public void MarkAsAnswered()
        {
            Answered = true;
        }
        public void Close()
        {
            IsClosed = true;
        }
    }

   
}
