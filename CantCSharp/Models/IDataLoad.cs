using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CantCSharp.Models
{
    interface IDataLoad
    {
        List<QuestionModel> LoadData(string fileroute);
    }
}
