using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ProTrack.NLP.NGrams
{
    public static class ReadGrams
    {
        
        public static List<string> ReadNGram(string text)
        {
            var gram = text.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList();
            return gram;
        }
    }
}
