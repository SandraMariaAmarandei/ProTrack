using System;
using ProTrack.NLP.Tokenization;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using ProTrack.NLP.NGrams;
using ProTrack.NLP.Stemming;

namespace ProTrack.AnalyzeFiles
{
    public class Analyze
    {
        private static readonly string OneGram = File.ReadAllText(@"F:\Master\Dizertatie\Work\N-grams\1-gram.txt");
        private static readonly string Treatments = File.ReadAllText(@"F:\Master\Dizertatie\Work\N-grams\treatment.txt");

        public List<List<string>> AnalyzeContextForOneGram()
        {
            var gramsList = GetGramsStem(OneGram);
            var contextContent = GetCauseList();
            var matchedContent = new List<List<string>>();

            foreach (var gram in gramsList)
            {
                var matched = contextContent.Where(x => x.Contains(gram)).ToList();
                if (matched.Count != 0)
                {
                    var occurency = GetOccurency(matched);
                    var cause = GetCause(occurency);
                    matchedContent.Add(cause);
                }
            }
            return matchedContent;
        }

        public List<Dictionary<int, string>> AnalyzeTreatments()
        {
            var treatmentList = GetTreatments();
            var contextContent = GetCauseList();
            var list = new List<Dictionary<int, string>>();

            foreach (var treatment in treatmentList)
            {
                var matched = contextContent.Where(x => x.Contains(treatment)).ToList();
                if (matched.Count != 0)
                {
                    var occurency = GetOccurency(matched);
                    list.Add(occurency);
                }
            }
            return list;
        }

        private List<string> GetCause(Dictionary<int, string> matchedDictionary)
        {
            var contextContent = GetCauseList();
            var expressionsList = new List<string>();

            foreach (var key in matchedDictionary.Keys)
            {
                var expressions = new List<string>();
                if (key - 3 >= 0 && key + 3 <= contextContent.Count)
                {
                    for (int i = key - 3; i <= key + 3; i++)
                    {
                        expressions.Add(contextContent[i]);
                    }
                    var information = string.Join(" ", expressions);
                    expressionsList.Add(information);
                }
                else
                {
                    expressionsList.Add(contextContent[key]);
                }
            }
            return expressionsList;
        }

        private Dictionary<int, string> GetOccurency(List<string> matched)
        {
            var contextContent = GetCauseList();
            var dictionary = new Dictionary<int, string>();
            for (int i = 0; i < contextContent.Count; i++)
            {
                for (int j = 0; j < matched.Count; j++)
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
                var fileContent = Regex.Replace(entitie.Cause.ToLower(), @"[^\da-zA-Z-]", " ");
                fileContent= fileContent.Trim();
                words = fileContent.Split(' ').Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
            }
            return words;
        }

        private string GetResultList()
        {
            var fileEntitities = GetFileEntities();
            var fileContent=String.Empty;
            foreach (var entitie in fileEntitities)
            {
                 fileContent = Regex.Replace(entitie.Result, @"[^\da-zA-Z-]", " ");
                fileContent = fileContent.Trim();
            }
            return fileContent;
        }

        private List<string> GetTreatments()
        {
            return ReadGrams.ReadNGram(Treatments);
        }
    }
}
