using System.Collections.Generic;
using System.IO;
using System.Linq;
using ProTrack.NLP.Tokenization;

namespace ProTrack.AnalyzeFiles
{

    public class Analyze
    {
        private static readonly string OneGram = File.ReadAllText(@"F:\Master\Dizertatie\Work\N-grams\1-gram.txt");
        private static readonly string Efficiency = File.ReadAllText(@"F:\Master\Dizertatie\Work\N-grams\efficiency.txt");
        private readonly FileProcessing _fileProcessing = new FileProcessing();

        public List<List<string>> AnalyzeContext()
        {
            var gramsList = _fileProcessing.GetGramsStem(OneGram);
            var causeContent = _fileProcessing.GetCauseList();
            var matchedContent = new List<List<string>>();

            foreach (var gram in gramsList)
            {
                var matched = causeContent.Where(x => x.Contains(gram)).ToList();
                if (matched.Count != 0)
                {
                    var occurency = _fileProcessing.GetOccurency(matched);
                    var cause = _fileProcessing.GetCause(occurency);
                    matchedContent.Add(cause);
                }
            }
            return matchedContent;
        }

        public List<Dictionary<int, string>> AnalyzeTreatments()
        {
            var treatmentList = _fileProcessing.GetTreatments();
            var causeContent = _fileProcessing.GetCauseList();
            var list = new List<Dictionary<int, string>>();

            foreach (var treatment in treatmentList)
            {
                var matched = causeContent.Where(x => x.Contains(treatment)).ToList();
                if (matched.Count != 0)
                {
                    var occurency = _fileProcessing.GetOccurency(matched);
                    list.Add(occurency);
                }
            }
            return list;
        }

        public void AnalyzeEfficiency()
        {
            var resulContent = _fileProcessing.GetResult();

        }
    }
}
