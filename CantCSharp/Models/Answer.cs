using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CantCSharp.Models
{
    public class Answer : IAnswer
    {
        int Id { get; }
        string TheAnswer { get; set; }

        bool IsSolution { get; set; }

        public Answer(int id, string theAnswer)
        {
            Id = id;
            TheAnswer = theAnswer;
            IsSolution = false;

        }

        public void MarkAsSolution()
        {
            IsSolution = true;
        }

    }
}
