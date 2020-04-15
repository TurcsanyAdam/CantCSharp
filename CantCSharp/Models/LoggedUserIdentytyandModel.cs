using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CantCSharp.Models
{
    public class LoggedUserIdentytyandModel
    {
        public User User { get; set; }
        public QuestionModel QuestionModel { get; set; }
        public IAnswer Answer { get; set; }

        public LoggedUserIdentytyandModel(User user, QuestionModel questionmodel)
        {
            User = user;
            QuestionModel = questionmodel;
        }

        public LoggedUserIdentytyandModel(User user, IAnswer answer)
        {
            User = user;
            Answer = answer;
        }
    }
}
