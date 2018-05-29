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

        public List<List<string>> AnalyzeContext()
        {
            var gramsList = GetGramsStem(OneGram);
            var contextContent = GetContext();
            var matchedContent = new List<List<string>>();

            foreach (var gram in gramsList)
            {
                var matched = contextContent.Where(x => x.Contains(gram)).ToList();
                if (matched.Count != 0)
                {
                    matchedContent.Add(matched);
                    //foreach (var word in matched)
                    //{
                    //var matchedPosition = FindAllIndex(contextContent, word);
                    //    positions.Add(matchedPosition);
                    //}
                    var caca = GetOccurency(matched);
                }
            }
            return matchedContent;
        }

        private Dictionary<int, string> GetOccurency(List<string> matched)
        {
            var contextContent = GetContext();
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

        public List<int> FindAllIndex<T>( List<T> container, string match)
        {
            var items = container.FindAll(m => m.Equals(match));
            List<int> indexes = new List<int>();
            foreach (var item in items)
            {
                indexes.Add(container.IndexOf(item));
            }

            return indexes;
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

        private List<string> GetContext()
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
