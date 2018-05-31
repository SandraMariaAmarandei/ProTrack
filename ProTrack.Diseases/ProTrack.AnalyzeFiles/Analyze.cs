using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ProTrack.AnalyzeFiles
{

    public class Analyze
    {
        private static readonly string OneGram = File.ReadAllText(@"F:\Master\Dizertatie\Work\N-grams\1-gram.txt");
        private static readonly string Efficiency = File.ReadAllText(@"F:\Master\Dizertatie\Work\N-grams\efficiency.txt");
        private readonly FileProcessing _fileProcessing = new FileProcessing();

        public List<List<List<string>>> AnalyzeContext()
        {
            var gramsList = _fileProcessing.GetGramsStem(OneGram);
            var causeContent = _fileProcessing.GetCauseList();
            var contextList = new List<List<List<string>>>();

            foreach (var content in causeContent)
            {
                var matchedContent = new List<List<string>>();
                foreach (var gram in gramsList)
                {
                    var matched = content.Where(x => x.Contains(gram)).ToList();
                    if (matched.Count != 0)
                    {
                        var occurency = _fileProcessing.GetOccurency(matched, content);
                        var cause = _fileProcessing.GetCause(occurency, content);
                        matchedContent.Add(cause);
                    }
                }
                contextList.Add(matchedContent);
            }

            return contextList;
        }

        public List<List<Dictionary<int, string>>> AnalyzeTreatments()
        {
            var treatmentList = _fileProcessing.GetTreatments();
            var causeContent = _fileProcessing.GetCauseList();
            var fileTreatment = new List<List<Dictionary<int, string>>>();
            foreach (var content in causeContent)
            {
                var list = new List<Dictionary<int, string>>();

                foreach (var treatment in treatmentList)
                {
                    var matched = content.Where(x => x.Contains(treatment)).ToList();
                    if (matched.Count != 0)
                    {
                        var occurency = _fileProcessing.GetOccurency(matched, content);
                        list.Add(occurency);
                    }
                }
                fileTreatment.Add(list);
            }
            return fileTreatment;
        }

        public List<List<List<string>>> AnalyzeEfficiency()
        {
            var resulContent = _fileProcessing.GetResult();
            var efficiencyList = _fileProcessing.GetGramsStem(Efficiency);
            var resultsList = new List<List<List<string>>>();

            foreach (var content in resulContent)
            {
                var matchedContent = new List<List<string>>();
                foreach (var gram in efficiencyList)
                {
                    var matched = content.Where(x => x.Contains(gram)).ToList();
                    if (matched.Count != 0)
                    {
                        var occurency = _fileProcessing.GetOccurency(matched, content);
                        var cause = _fileProcessing.GetCause(occurency, content);
                        matchedContent.Add(cause);
                    }
                }
                resultsList.Add(matchedContent);
            }
            return resultsList;

        }
    }
}
