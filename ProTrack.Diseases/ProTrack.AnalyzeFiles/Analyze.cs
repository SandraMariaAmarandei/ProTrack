using System;
using ProTrack.NLP.Tokenization;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ProTrack.NLP.Stemming;

namespace ProTrack.AnalyzeFiles
{
    public class Analyze
    {
        private static readonly string OneGram = File.ReadAllText(@"F:\Master\Dizertatie\Work\N-grams\1-gram.txt");
        private static readonly string TwoGram = File.ReadAllText(@"F:\Master\Dizertatie\Work\N-grams\2-gram.txt");
        private static readonly string Treatments = File.ReadAllText(@"F:\Master\Dizertatie\Work\N-grams\treatment.txt");

        public List<string> GetCause()
        {

            return null;
        }

        private List<Dictionary<int, string>> AnalyzeContextForOneGram()
        {
            var gramsList = GetGramsStem(OneGram);
            var contextContent = GetCauseList();
            var matchedContent = new List<Dictionary<int, string>>();

            foreach (var gram in gramsList)
            {
                var matched = contextContent.Where(x => x.Contains(gram)).ToList();
                if (matched.Count != 0)
                {
                    var occurency = GetOccurency(matched);
                    matchedContent.Add(occurency);
                }
            }
            return matchedContent;
        }

        private Dictionary<int, string> GetOccurency(List<string> matched)
        {
            var contextContent = GetCauseList();
            var dictionary = new Dictionary<int, string>();
            for (int i=0; i < contextContent.Count; i++)
            {
                for (int j=0; j<matched.Count;j++)
                {
                    if (contextContent[i] == matched[j])
                    {
                       dictionary.Add(i, matched[j]);
                       break;
                    }
                }
            }
            return dictionary;
        }

        private List<RelationEntity> GetFileEntities()
        {
            var split = new Split();
            return split.SplitFile();
        }

        private List<string> GetGramsStem(string gram)
        {
            return WordStem.FindGramStem(gram);
        }

        private List<string> GetCauseList()
        {
            var fileEntitities = GetFileEntities();
            var words = new List<string>();

            foreach (var entitie in fileEntitities)
            {
                words = entitie.Cause.Split(' ').ToList();
            }
            return words;
        }
    }
}
