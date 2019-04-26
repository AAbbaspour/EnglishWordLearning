using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnglishWordLearning
{
    public class Word
    {
        public String Text { get; set; }
        public String Meaning { get; set; }

        public int TextLength { get { return this.Text.Length; } }

        public int Count { get; set; }



    }
}
