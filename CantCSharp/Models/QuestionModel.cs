using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace CantCSharp.Models
{
    public class QuestionModel : IQuestion
    {
        int QuestionID { get; set; }
        DateTime PostTime { get; set; }
        int ViewNumber { get; set; }
        int VoteNumber { get; set; }
        int QuestionTitle { get; set; }
        string QuestionMessage { get; set; }
        List <string> Answer { get; set; }
       

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
