using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace CantCSharp.Models
{
    public class QuestionModel : IQuestion
    {
        public string User { get; private set; }
        public int QuestionID { get; set; }
        public DateTime PostTime { get; set; }
        public int ViewNumber { get; set; }
        public int VoteNumber { get; set; }
        public string QuestionTitle { get; set; }
        public string QuestionMessage { get; set; }
        public List<IAnswer> AnswerList { get; set; }
        public  bool Answered { get; private set; }
        public bool IsClosed { get; private set; }
        public List<Comment> QuestionComments { get; set; }





        public QuestionModel(int questionid, DateTime date, int viewNum, int voteNum,
            string questionTitle, string questionMessage, string user)
        {
            ViewNumber = viewNum;
            VoteNumber = voteNum;
            QuestionID = questionid;
            QuestionTitle = questionTitle;
            QuestionMessage = questionMessage;
            User = user;
            AnswerList = new List<IAnswer>();
            PostTime = date;
            Answered = false;
            IsClosed = false;
            QuestionComments = new List<Comment>();
        }


        public QuestionModel(string user, int questionid, string postTime, int viewNumber, int voteNumber, string questionTitle, string questionMessage, bool answered, bool isClosed)
        {

            ViewNumber = viewNumber;
            VoteNumber = voteNumber;
            QuestionID = questionid;
            QuestionTitle = questionTitle;
            QuestionMessage = questionMessage;
            User = user;
            AnswerList = new List<IAnswer>();
            PostTime = Convert.ToDateTime(postTime);
            Answered = answered;
            IsClosed = isClosed;
            QuestionComments = new List<Comment>();
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
