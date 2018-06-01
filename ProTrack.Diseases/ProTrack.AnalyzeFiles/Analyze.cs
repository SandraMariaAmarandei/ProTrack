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

        public List<string> Context()
        {
            var causeContent = _fileProcessing.GetMotivationList();
            var causeList = new List<string>();
            var gramsList = _fileProcessing.GetGramsStem(OneGram);

            foreach (var content in causeContent)
            {
                if (content.Count <= 35)
                {
                    causeList.Add(string.Join(" ", content));
                }
                else
                {
                    var matchedContent = new List<string>();
                    foreach (var gram in gramsList)
                    {
                        var matched = content.Where(x => x.Contains(gram)).ToList();
                        if (matched.Count != 0)
                        {
                            var occurency = _fileProcessing.GetOccurency(matched, content);
                            var causes = string.Join("\n",_fileProcessing.GetCause(occurency, content));
                            matchedContent.Add(causes);
                        }
                    }
                    causeList.Add(string.Join("\n\n", matchedContent));
                }
            }
            return causeList;
        }

        public List<string> AnalyzeTreatments()
        {
            var treatmentList = _fileProcessing.GetTreatments();
            var causeContent = _fileProcessing.GetCauseList();
            var fileTreatment = new List<string>();
            foreach (var content in causeContent)
            {
                var list = new List<string>();

                foreach (var treatment in treatmentList)
                {
                    var matched = content.Where(x => x.Contains(treatment)).ToList();
                    if (matched.Count != 0)
                    {
                        var occurency = _fileProcessing.GetOccurency(matched, content);
                        list.Add(string.Join("\n", occurency.Values.ToList()));
                    }
                }
                fileTreatment.Add(string.Join("; ",list));
            }
            return fileTreatment;
        }

        public List<string> AnalyzeEfficiency()
        {
            var resulContent = _fileProcessing.GetResult();
            var efficiencyList = _fileProcessing.GetGramsStem(Efficiency);
            var resultsList = new List<string>();

            foreach (var content in resulContent)
            {
                var matchedContent = new List<string>();
                foreach (var gram in efficiencyList)
                {
                    var matched = content.Where(x => x.Contains(gram)).ToList();
                    if (matched.Count != 0)
                    {
                        var occurency = _fileProcessing.GetOccurency(matched, content);
                        var cause = string.Join("\n", _fileProcessing.GetCause(occurency, content));
                        matchedContent.Add(cause);
                    }
                }
                resultsList.Add(string.Join("\n\n", matchedContent));
            }
            return resultsList;
        }
    }
}
