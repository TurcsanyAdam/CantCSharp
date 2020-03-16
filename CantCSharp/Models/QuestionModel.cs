﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace CantCSharp.Models
{
    public class QuestionModel : IQuestion
    {
        public int QuestionID { get; set; }
        public string Question { get; set; }
        public List <string> Answer { get; set; }
        public DateTime PostTime { get; set; }

        
    }

   
}
