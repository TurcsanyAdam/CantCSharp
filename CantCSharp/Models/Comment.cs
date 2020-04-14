using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CantCSharp.Models
{
    public class Comment
    {
        public string Message { get; set; }
        public DateTime SubmissionTime { get; set; }
        public int EditedNumber { get; set; }
        public string User { get; private set; }
        public int Id { get; private set; }
        public int AnswerOrQuestionID { get; private set; }

        public int UserID { get; set; }


        public Comment(string message,DateTime submissionTime,int editedNumber,string user,int id,int answerOrQuestionID, int userID)
        {
            Message = message;
            SubmissionTime = submissionTime;
            EditedNumber = editedNumber;
            User = user;
            Id = id;
            AnswerOrQuestionID = answerOrQuestionID;
            UserID = userID;
        }
    }
}
