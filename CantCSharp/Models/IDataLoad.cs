﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CantCSharp.Models
{
    public interface IDataLoad
    {
        List<QuestionModel> QuestionList { get; set; }
        List<QuestionModel> LoadData(string fileroute,string justsomething);
        void AddQuestion(string title, string message, string user);
         

    }
}
