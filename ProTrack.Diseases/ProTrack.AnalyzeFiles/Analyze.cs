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

        public List<RelationEntity> GetFileEntities()
        {
            var split = new Split();
            return split.SplitFile();
        }

        public List<string> GetGramsStem(string gram)
        {
            return WordStem.FindGramStem(gram);
        }

        public List<int> AnalyzeContext()
        {
            var gramsList = GetGramsStem(OneGram);
            var contextContent = GetContext();
            var contextMatches = new List<int>();

            foreach (var gram in gramsList)
            {
                contextMatches.Add(contextContent.BinarySearch(gram));
            }
            return contextMatches;
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
