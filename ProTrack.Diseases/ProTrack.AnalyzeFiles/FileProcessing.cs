using System;
using System.Collections;
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
        
        public List<string> GetCause(Dictionary<int, string> matchedDictionary, List<string>content)
        {
            var expressionsList = new List<string>();

            foreach (var key in matchedDictionary.Keys)
            {
                    var expressions = new List<string>();
                    if (key - 1 >= 0 && key < content.Count)
                    {
                        for (int i = key - 1 ; i <= key; i++)
                        {
                            expressions.Add(content[i]);
                        }
                        var information = string.Join(" ", expressions);
                        expressionsList.Add(information);
                    }
                    else
                    {
                        expressionsList.Add(content[key]);
                    }
            }
            return expressionsList;
        }

        public List<string> GetResultCause(Dictionary<int, string> matchedDictionary, List<string> content)
        {
            var expressionsList = new List<string>();

            foreach (var key in matchedDictionary.Keys)
            {
                var expressions = new List<string>();
                if (key - 2 >= 0 && key < content.Count)
                {
                    for (int i = key - 2; i <= key; i++)
                    {
                        expressions.Add(content[i]);
                    }
                    var information = string.Join(" ", expressions);
                    expressionsList.Add(information);
                }
                else
                {
                    expressionsList.Add(content[key]);
                }
            }
            return expressionsList.Distinct().ToList();
        }

        public Dictionary<int, string> GetOccurency(List<string> matched, List<string> content)
        {
            var dictionary = new Dictionary<int, string>();

                for (int i = 0; i < content.Count; i++)
                {
                    for (int j = 0; j < matched.Count; j++)
                    {
                        if (content[i] == matched[j])
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

        public List<string> GetMotivation()
        {
            var split = new Split();
            return split.TakeMotivation();
        }

        public List<string> GetGramsStem(string gram)
        {
            return WordStem.FindGramStem(gram);
        }

        public List<List<string>> GetMotivationList()
        {
            var fileEntitities = GetMotivation();
            var fileWords = new List<List<string>>();

            foreach (var entitie in fileEntitities)
            {
                var fileContent = Regex.Replace(entitie.ToLower(), @"[^a-zA-Z]", " ");
                fileContent = fileContent.Trim();
                var words = fileContent.Split(' ').Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
                fileWords.Add(words);
            }
            return fileWords;
        }

        public List<List<string>> GetCauseList()
        {
            var fileEntitities = GetFileEntities();
            var fileWords = new List<List<string>>();
            foreach (var entitie in fileEntitities)
            {
                var fileContent = Regex.Replace(entitie.Cause.ToLower(), @"[^\da-zA-Z-]", " ");
                fileContent = fileContent.Trim();
                var words = fileContent.Split(' ').Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
                fileWords.Add(words);
            }
            return fileWords;
        }

        public List<List<string>> GetResult()
        {
            var fileEntitities = GetFileEntities();
            var fileResult = new List<List<string>>();
            var garbages = new List<string>
            {
                "the", "and", "mg", "an", "a", "of", "cr", "mg", "day","odds", "ratio", "at",
                "p", "n", "to", "in", "for", "vs", "with", "was", "but", "those"
            };
            foreach (var entitie in fileEntitities)
            {
                var fileResultContent = Regex.Replace(entitie.Result.ToLower(), @"[^a-zA-Z]", " ");
                fileResultContent = fileResultContent.Trim();
                var words = fileResultContent.Split(' ').Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
                foreach (var grb in garbages)
                {
                    words = words.Where(s => !s.Equals(grb)).ToList();
                }
                fileResult.Add(words);
            }
            return fileResult;
        }

        public List<string> GetTreatments()
        {
            return ReadGrams.ReadNGram(Treatments);
        }
    }
}
