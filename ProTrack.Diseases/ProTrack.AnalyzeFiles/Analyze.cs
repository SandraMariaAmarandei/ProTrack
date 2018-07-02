using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Iveonik.Stemmers;
using Microsoft.SqlServer.Server;
using ProTrack.NLP.NGrams;

namespace ProTrack.AnalyzeFiles
{

    public class Analyze
    {
        private static readonly string OneGram = File.ReadAllText(@"F:\Master\Dizertatie\Work\N-grams\1-gram.txt");
        private static readonly string Efficiency = File.ReadAllText(@"F:\Master\Dizertatie\Work\N-grams\efficiency.txt");

        private static readonly string Medication = File.ReadAllText(@"F:\Master\Dizertatie\Work\N-grams\Context\medication.txt");
        private static readonly string Seizures = File.ReadAllText(@"F:\Master\Dizertatie\Work\N-grams\Context\seizures.txt");
        private static readonly string Chronic = File.ReadAllText(@"F:\Master\Dizertatie\Work\N-grams\Context\chronic_disorder.txt");
        private static readonly string Neurological = File.ReadAllText(@"F:\Master\Dizertatie\Work\N-grams\Context\neurological.txt");
        private static readonly string Diet = File.ReadAllText(@"F:\Master\Dizertatie\Work\N-grams\Context\diet.txt");
        private static readonly string Surgery = File.ReadAllText(@"F:\Master\Dizertatie\Work\N-grams\Context\surgery.txt");
        private static readonly string Psychiatric = File.ReadAllText(@"F:\Master\Dizertatie\Work\N-grams\Context\psychiatric.txt");

        private readonly FileProcessing _fileProcessing = new FileProcessing();

        public List<string> AnalyzeContext()
        {
            var causeContent = _fileProcessing.GetMotivationList();
            var causeList = new List<string>();
            var gramsList = _fileProcessing.GetGramsStem(OneGram);

            foreach (var content in causeContent)
            {
                if (content.Count <= 30)
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
                            var causes = string.Join("**", _fileProcessing.GetCause(occurency, content));
                            matchedContent.Add(causes);
                        }
                    }
                    causeList.Add(string.Join("##", matchedContent));
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
                        list.Add(string.Join(" ", occurency.Values.Distinct().ToList()));
                    }
                }
                if (!list.Any())
                {
                    list.Add("experiment");
                }
                fileTreatment.Add(string.Join(" & ", list));
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
                        var cause = string.Join("**", _fileProcessing.GetResultCause(occurency, content));
                        matchedContent.Add(cause);
                    }
                }
                if (!matchedContent.Any())
                {
                    matchedContent.Add("inconclusive");
                }
                resultsList.Add(string.Join("##", matchedContent));
            }
            return resultsList;
        }

        public List<string> AnalyzeDiseaseLevel()
        {
            var texts = _fileProcessing.GetAllText();
            var diseaseGrams = _fileProcessing.GetDiseaseLevels();
            var list = new List<string>();
            var level = new List<string>();
            foreach (var text in texts)
            {
                foreach (var gram in diseaseGrams)
                {
                    var match = text.Contains(gram) ? gram : "undefined";
                    list.Add(match);
                }
                var unique = list.Distinct().ToList();
                if (unique.Count > 1)
                {
                    unique.Remove("undefined");
                }
                level.Add(string.Join(" ", unique));
                list = new List<string>();
            }
            return level;
        }

        //reprocessing the level of disease
        public List<string> ReprocessingDiseaseLevel(List<string> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Equals("undefined"))
                {
                    list[i] = "unknown";
                }
                else if (list[i].StartsWith("complex") || list[i].StartsWith("simple") || list[i].StartsWith("focal") ||
                         list[i].StartsWith("symptomatic"))
                {
                    list[i] = "focal";
                }
                else
                {
                    list[i] = "generalized";
                }
            }
            return list;
        }

        //reprocessing the efficiency
        public List<string> ReprocessingEfficiancy()

        {
            string[] ineffective = {"low", "less", "wors" };
            string[] neutral = { "consist", "toler", "indic", "achiev", "predict", "therapi", "econom" };
            string[] efficiently = { "effect", "remiss", "high", "safe", "advantag", "efficaci", "improv", "promis", "reduc", "help", "suppress", "favor", "applic" };

            var resulContent = _fileProcessing.GetResult();

            List<string> categoryList = new List<string>();
            int inf;
            int neut;
            int eff;

            for (int i = 0; i < resulContent.Count; i++)
            {
                inf = neut = eff = 0;
                
                foreach (string item in resulContent[i])
                {
                    var wordStem = FindGramStem(item);
                    if (ineffective.Contains(wordStem))
                    {
                        inf++;
                        continue;
                    }
                    if (neutral.Contains(wordStem))
                    {
                        neut++;
                        continue;
                    }
                    if (efficiently.Contains(wordStem))
                    {
                        eff++;
                        continue;
                    }
                }
                if (inf == eff && eff == neut && neut == 0)
                {
                    categoryList.Add("inconclusive");
                }
                else
                {
                    if (inf > neut && inf > eff)
                    {
                        categoryList.Add("ineffective");
                    }
                    if (neut > inf && neut > eff)
                    {
                        categoryList.Add("neutral");
                    }
                    if (eff > neut && eff > inf)
                    {
                        categoryList.Add("efficient");
                    }
                    if((inf==eff || inf==neut )&& inf!=0)
                    {
                        categoryList.Add("ineffective");
                    }
                    if (neut == eff && neut != 0)
                    {
                        categoryList.Add("efficient");
                    }
                    if (inf == eff && eff == neut && neut != 0)
                    {
                        categoryList.Add("ineffective");
                    }
                }
            }
            
            return categoryList;
        }

        //reprocessing the context
        public List<string> ReprocessingContext()
        {
            var causeContent = _fileProcessing.GetMotivationList();

            var medication = _fileProcessing.GetGramsStem(Medication);
            var seizures = _fileProcessing.GetGramsStem(Seizures);
            var chronic = _fileProcessing.GetGramsStem(Chronic);
            var neuro = _fileProcessing.GetGramsStem(Neurological);
            var diet = _fileProcessing.GetGramsStem(Diet);
            var surgery = _fileProcessing.GetGramsStem(Surgery);
            var psycho = _fileProcessing.GetGramsStem(Psychiatric);

            List<string> categoryList = new List<string>();
            int med, seiz, chr, nr, dt, srg, psych;
            int maxim;
            int minim;
            string category = String.Empty;
            string categoryMinim=String.Empty;
            for (int i = 0; i < causeContent.Count; i++)
            {
                med = seiz = chr = nr = dt = srg = psych = 0;
                maxim = 0;
                minim = 1000;
                
                foreach (string item in causeContent[i])
                {
                    

                    var wordStem = FindGramStem(item);
                    if (medication.Contains(wordStem))
                    {
                        med++;
                        if (maxim < med)
                        {
                            maxim = med;
                            category = "medication";
                        }
                        if (minim > med)
                        {
                            minim = med;
                            categoryMinim = "medication";
                        }
                        continue;
                    }
                    if (seizures.Contains(wordStem))
                    {
                        seiz++;
                        if (maxim < seiz)
                        {
                            maxim = seiz;
                            category = "seizures";
                        }
                        if (minim > seiz)
                        {
                            minim = seiz;
                            categoryMinim = "seizures";
                        }
                        continue;
                    }
                    if (chronic.Contains(wordStem))
                    {
                        chr++;
                        if (maxim < chr)
                        {
                            maxim = chr;
                            category = "chronic disorder";
                        }
                        if (minim > chr)
                        {
                            minim = chr;
                            categoryMinim = "chronic disorder";
                        }
                        continue;
                    }
                    if (neuro.Contains(wordStem))
                    {
                        nr++;
                        if (maxim < nr)
                        {
                            maxim = nr;
                            category = "neurological";
                        }
                        if (minim > nr)
                        {
                            minim = nr;
                            categoryMinim = "neurological";
                        }
                        continue;
                    }
                    if (diet.Contains(wordStem))
                    {
                        dt++;
                        if (maxim < dt)
                        {
                            maxim = dt;
                            category = "diet";
                        }
                        if (minim > dt)
                        {
                            minim = dt;
                            categoryMinim = "diet";
                        }
                        continue;
                    }
                    if (surgery.Contains(wordStem))
                    {
                        srg++;
                        if (maxim < srg)
                        {
                            maxim = srg;
                            category = "surgery";
                        }
                        if (minim > srg)
                        {
                            minim = srg;
                            categoryMinim = "surgery";
                        }
                        continue;
                    }
                    if (psycho.Contains(wordStem))
                    {
                        psych++;
                        if (maxim < psych)
                        {
                            maxim = psych;
                            category = "psychiatric";
                        }
                        if (minim > psych)
                        {
                            minim = psych;
                            categoryMinim = "psychiatric";
                        }
                        continue;
                    }
                }
                if (categoryMinim.Equals("diet"))
                {
                    categoryList.Add("diet");
                }
                else if (categoryMinim.Equals("neurological"))
                {
                    categoryList.Add("neurological");
                }
                else
                {

                    if (med == seiz && seiz == chr && chr == nr && nr == dt && dt == srg && srg == psych && psych == 0)
                    {
                        categoryList.Add("study");
                    }
                    else
                    {
                        categoryList.Add(category);
                    }
                }

                //Console.WriteLine(med);
                //Console.WriteLine(seiz);
                //Console.WriteLine(chr);
                //Console.WriteLine(nr);
                //Console.WriteLine(dt);
                //Console.WriteLine(srg);
                //Console.WriteLine(psych);
                //Console.WriteLine("_______________________________________________________");
            }

            return categoryList;
        }

        private string FindGramStem(string word)
        {
            var englishStemmer = new EnglishStemmer();

            var stem = englishStemmer.Stem(word);
               
            return stem;
        }

    }
}
