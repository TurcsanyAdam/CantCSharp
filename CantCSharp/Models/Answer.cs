using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CantCSharp.Models
{
    public class Answer : IAnswer
    {
        int Id { get; }
        DateTime PostTime { get; set; }
        int VoteNumber { get; set; }
        int QuestionID { get; set; }
        string AnswerMessage { get; set; }

        bool IsSolution { get; set; }

        public Answer(int id, string theAnswer)
        {
            Id = id;
            AnswerMessage = theAnswer;
            IsSolution = false;

        }

        public void MarkAsSolution()
        {
            IsSolution = true;
        }

    }
}
