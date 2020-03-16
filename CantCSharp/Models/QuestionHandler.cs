using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CantCSharp.Models
{
    public class QuestionHandler
    {
        public List<QuestionModel> questions { get; set; }

        public QuestionHandler()
        {
            questions = new List<QuestionModel>();
        }
    }
}
