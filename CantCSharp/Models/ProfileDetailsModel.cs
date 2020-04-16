using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CantCSharp.Models
{
    public class ProfileDetailsModel
    {
        public User User { get; private set; }
        public List<QuestionModel> QuestionsList { get; private set; }

        public ProfileDetailsModel(User user, List<QuestionModel> questionsList)
        {
            User = user;
            QuestionsList = questionsList;
        }
    }
}
