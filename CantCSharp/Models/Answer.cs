using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CantCSharp.Models
{
    public class Answer : IAnswer
    {
        public string User { get; }
        public int Id { get; }
        public DateTime PostTime { get; set; }
        public int VoteNumber { get; set; }
        public int QuestionID { get; set; }
        public string AnswerMessage { get; set; }
        public string ImageSource { get; set; }
        public string[] Link { get; set; }
        public bool IsSolution { get; set; }
        public List<Comment> AnswerComments { get; set; }
        public int UserID { get; set; }

        public Answer(int id,string user, string theAnswer,string imageSource, int QuesitonId, int userID)
        {
            this.User = user;
            this.QuestionID = QuesitonId;
            Id = id;
            AnswerMessage = theAnswer;
            IsSolution = false;
            PostTime = DateTime.Now;
            ImageSource = imageSource;
            AnswerComments = new List<Comment>();
            UserID = userID;
        }
        public Answer(string user, int id, DateTime postTime, int voteNumber, int questionID, string theAnswer, string imageSource, string link, bool isSolution, int userID)
        {
            User = user;
            Id = id;
            PostTime = postTime;
            VoteNumber = voteNumber;
            QuestionID = questionID;
            AnswerMessage = theAnswer;
            ImageSource = imageSource;
            Link = new string[] { link }; 
            IsSolution = isSolution;
            AnswerComments = new List<Comment>();
            UserID = userID;
        }
    


        public void MarkAsSolution()
        {
            IsSolution = true;
        }

        public int CompareTo(IAnswer otherAnswer)
        {
            // implement your custom comparison here...

            return VoteNumber.CompareTo(otherAnswer.VoteNumber); // e.g.
        }
        public override string ToString()
        {
            return PostTime + " " + User + ": " + AnswerMessage;
        }
    }
}
