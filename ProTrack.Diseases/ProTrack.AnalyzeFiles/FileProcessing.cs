using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using ProTrack.NLP.NGrams;
using ProTrack.NLP.Stemming;
using ProTrack.NLP.Tokenization;

namespace ProTrack.AnalyzeFiles
{
    public class FileProcessing
    {
        private static readonly string Treatments = File.ReadAllText(@"F:\Master\Dizertatie\Work\N-grams\treatment.txt");
        
        public List<string> GetCause(Dictionary<int, string> matchedDictionary)
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

        public Dictionary<int, string> GetOccurency(List<string> matched)
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

        public List<RelationEntity> GetFileEntities()
        {
            var split = new Split();
            return split.SplitFile();
        }

        public List<string> GetGramsStem(string gram)
        {
            return WordStem.FindGramStem(gram);
        }

        public List<string> GetCauseList()
        {
            var fileEntitities = GetFileEntities();
            var words = new List<string>();

            foreach (var entitie in fileEntitities)
            {
                var fileContent = Regex.Replace(entitie.Cause.ToLower(), @"[^\da-zA-Z-]", " ");
                fileContent = fileContent.Trim();
                words = fileContent.Split(' ').Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
            }
            return words;
        }

        public string GetResult()
        {
            var fileEntitities = GetFileEntities();
            var fileContent = String.Empty;
            foreach (var entitie in fileEntitities)
            {
                fileContent = Regex.Replace(entitie.Result, @"[^\da-zA-Z-]", " ");
                fileContent = fileContent.Trim();
            }
            return fileContent;
        }

        public List<string> GetTreatments()
        {
            return ReadGrams.ReadNGram(Treatments);
        }
    }
}
