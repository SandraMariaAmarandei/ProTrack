using System.Collections.Generic;
using Iveonik.Stemmers;
using ProTrack.NLP.NGrams;

namespace ProTrack.NLP.Stemming
{
    public static class WordStem
    {
        public static List<string> FindGramStem(string gramList)
        {
            var grams = ReadGrams.ReadNGram(gramList);
            var gramStemList = new List<string>();
            var englishStemmer = new EnglishStemmer();

            foreach (var gram in grams)
            {
                var stem = englishStemmer.Stem(gram);
                gramStemList.Add(stem);
            }
            return gramStemList;
        }
    }
}
