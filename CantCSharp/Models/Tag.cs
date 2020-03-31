using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CantCSharp.Models
{
    public class Tag
    {
        public string  TagName { get; set; }

        public Tag(string tagName)
        {
            TagName = tagName;
        }
    }
}
