using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CantCSharp.Models
{
    public interface IAnswer
    {
        int VoteNumber { get; set; }
        int QuestionID { get; set; }
        List<Comment> AnswerComments { get; set; }

        void MarkAsSolution();
    }
}
