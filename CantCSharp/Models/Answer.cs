﻿using System;
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

        public bool IsSolution { get; set; }

        public Answer(int id,string User, string theAnswer,string imageSource, int QuesitonId)
        {
            this.User = User;
            this.QuestionID = QuestionID;
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
