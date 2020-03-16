using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace CantCSharp.Models
{
    public class QuestionModel : IQuestion
    {
        public int QuestionID { get; set; }
        public DateTime PostTime { get; set; }
        public int ViewNumber { get; set; }
        public int VoteNumber { get; set; }
        public string QuestionTitle { get; set; }
        public string QuestionMessage { get; set; }
        public List<IAnswer> AnswerList { get; set; }
        public  bool Answered { get; private set; }
        public bool IsClosed { get; private set; }





        public QuestionModel(int questionid,string question,string questiontitle)
        {
            ViewNumber = 0;
            VoteNumber = 0;
            QuestionID = questionid;
            QuestionTitle = questiontitle;
            QuestionMessage = question;
            AnswerList = new List<IAnswer>();
            PostTime = DateTime.Now;
            Answered = false;
            IsClosed = false;
        }

        public void AddAnswer(IAnswer answer)
        {
            AnswerList.Add(answer);
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
