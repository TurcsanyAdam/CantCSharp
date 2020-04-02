using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CantCSharp.Models
{
    public class SearchDataModel
    {
        public List<QuestionModel> ResultList { get; private set; }
        public string HighlightPattern { get; private set; }

        public SearchDataModel(List<QuestionModel> resultList, string highlightPattern)
        {
            HighlightPattern = highlightPattern;
            ResultList = resultList;
        }
    }
}
