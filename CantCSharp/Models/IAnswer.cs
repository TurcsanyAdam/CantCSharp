﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CantCSharp.Models
{
    public interface IAnswer
    {
        int VoteNumber { get; set; }

        void MarkAsSolution();
    }
}
