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

        public List<QuestionModel> QuestionModelList { get; set; }

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
        public LoggedUserIdentytyandModel(User user, List<QuestionModel> questionModel)
        {
            User = user;
            QuestionModelList = questionModel;
        }
        public LoggedUserIdentytyandModel(List<QuestionModel> questionModel)
        {
            
            QuestionModelList = questionModel;
        }
    }
}
