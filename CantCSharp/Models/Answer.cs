using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CantCSharp.Models
{
    public class Answer : IAnswer
    {
        string User { get; }
        public int Id { get; }
        DateTime PostTime { get; set; }
        int VoteNumber { get; set; }
        int QuestionID { get; set; }
        string AnswerMessage { get; set; }
        string ImageSource { get; set; }

        bool IsSolution { get; set; }

        public Answer(int id,string User, string theAnswer,string imageSource)
        {
            this.User = User;
            Id = id;
            AnswerMessage = theAnswer;
            IsSolution = false;
            PostTime = DateTime.Now;
            ImageSource = imageSource;


        }

        public void MarkAsSolution()
        {
            IsSolution = true;
        }
        public override string ToString()
        {
            return PostTime + " " + User + ": " + AnswerMessage;
        }
    }
}
