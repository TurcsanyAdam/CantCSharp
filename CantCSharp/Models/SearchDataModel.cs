using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CantCSharp.Models
{
    public class SearchDataModel
    {
        public Dictionary<QuestionModel, List<Answer>> ResultDict { get; private set; }
        public string HighlightPattern { get; private set; }

        public SearchDataModel(Dictionary<QuestionModel, List<Answer>> resultDict, string highlightPattern)
        {
            HighlightPattern = highlightPattern;
            ResultDict = resultDict;
        }
    }
}
