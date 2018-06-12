using System;
using System.Collections.Generic;
using System.Linq;

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
