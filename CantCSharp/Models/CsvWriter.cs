﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CantCSharp.Models
{
    public class CsvWriter
    {
        public List<QuestionModel> QuestionList;

        public CsvWriter(List<QuestionModel> QuestionList)
        {
            this.QuestionList = QuestionList;
        }

        public void WriteQuestions(string path)
        {
            using (var w = new StreamWriter(path))
            {
                foreach (QuestionModel question in QuestionList)
                {
                    string user = question.User;
                    int questionID = question.QuestionID;
                    DateTime dateTime = question.PostTime;
                    int viewNumber = question.ViewNumber;
                    int voteNumber = question.VoteNumber;
                    string questionTitle = question.QuestionTitle;
                    string questionMessage = question.QuestionMessage;
                    bool answer = question.Answered;
                    bool IsClosed = question.IsClosed;
                    var line = string.Format($"{user};{questionID};{dateTime};{viewNumber};{voteNumber};{questionTitle};{questionMessage};{answer};{IsClosed}");
                    w.WriteLine(line);
                    w.Flush();
                }
            }
        }

        public void WriteAnswers(string path)
        {
            using (var w = new StreamWriter(path))
            {
                foreach(QuestionModel question in QuestionList)
                {
                    foreach(Answer answer in question.AnswerList)
                    {
                        var link = "";
                        if(answer.Link != null)
                        {
                            link = answer.Link[0];
                        }
                         
                        var line = string.Format($"{answer.User};{answer.Id};{answer.PostTime};{answer.VoteNumber};{question.QuestionID};{answer.AnswerMessage};{answer.ImageSource};{link};{answer.IsSolution}");
                        w.WriteLine(line);
                        w.Flush();
                    }
                }
            }

        }

    }
}