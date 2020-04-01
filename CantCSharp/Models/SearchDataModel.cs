using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CantCSharp.Models
{
    public class SearchDataModel
    {
        public SortedDictionary<QuestionModel, List<Answer>> ResultDict { get; private set; }
        public string HighlightPattern { get; private set; }

        public SearchDataModel(SortedDictionary<QuestionModel, List<Answer>> resultDict, string highlightPattern)
        {
            HighlightPattern = highlightPattern;
            ResultDict = resultDict;
        }
    }
}
