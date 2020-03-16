using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CantCSharp.Models
{
    interface IQuestion
    {
        void AddAnswer(IAnswer answer);
        void MarkAsAnswered();
        void Close();
    }
}
