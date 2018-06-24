using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Iveonik.Stemmers;
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
                    list.Add("study");
                }
                fileTreatment.Add(String.Join("##", list));
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
                    list[i] = "focal epilepsy";
                }
                else
                {
                    list[i] = "generalized epilepsy";
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
                        categoryList.Add("efficiency");
                    }
                    if((inf==eff || inf==neut )&& inf!=0)
                    {
                        categoryList.Add("ineffective");
                    }
                    if (neut == eff && neut != 0)
                    {
                        categoryList.Add("efficiency");
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

            for (int i = 0; i < causeContent.Count; i++)
            {
                med = seiz = chr = nr = dt = srg = psych = 0;

                foreach (string item in causeContent[i])
                {
                    var wordStem = FindGramStem(item);
                    if (medication.Contains(wordStem))
                    {
                        med++;
                        continue;
                    }
                    if (seizures.Contains(wordStem))
                    {
                        seiz++;
                        continue;
                    }
                    if (chronic.Contains(wordStem))
                    {
                        chr++;
                        continue;
                    }
                    if (neuro.Contains(wordStem))
                    {
                        nr++;
                        continue;
                    }
                    if (diet.Contains(wordStem))
                    {
                        dt++;
                        continue;
                    }
                    if (surgery.Contains(wordStem))
                    {
                        srg++;
                        continue;
                    }
                    if (psycho.Contains(wordStem))
                    {
                        psych++;
                        continue;
                    }
                }
                if (med == seiz && seiz == chr && chr == nr && nr == dt && dt == srg && srg == psych && psych == 0)
                {
                    categoryList.Add("study");
                }
                else
                {
                    if (med > seiz && med > chr && med > nr && med > dt && med > srg && med > psych)
                    {
                        categoryList.Add("medication");
                    }
                    if (seiz > med  && seiz > chr && seiz > nr && seiz > dt && seiz > srg && seiz > psych)
                    {
                        categoryList.Add("seizures");
                    }
                    if (chr > seiz && chr > med && chr > nr && chr > dt && chr > srg && chr > psych)
                    {
                        categoryList.Add("chronic disorder");
                    }
                    if (nr > seiz && nr > chr && nr > med && nr > dt && nr > srg && nr > psych)
                    {
                        categoryList.Add("neurological");
                    }
                    if (dt > seiz && dt > chr && dt > nr && dt > med && dt > srg && dt > psych)
                    {
                        categoryList.Add("diet");
                    }
                    if (srg > seiz && srg > chr && srg > nr && srg > dt && srg > med && srg > psych)
                    {
                        categoryList.Add("surgery");
                    }
                    if (psych > seiz && psych > chr && psych > nr && psych > dt && psych > srg && psych > med)
                    {
                        categoryList.Add("psychiatric");
                    }
                    //if ((nr == srg || med == srg || seiz == srg || chr == srg || psych == srg) && srg != 0)
                    //{
                    //    categoryList.Add("surgery");
                    //}
                    //if ((psych == seiz || med == psych) && psych != 0)
                    //{
                    //    categoryList.Add("psychiatric");
                    //}
                    //if ( med == nr && seiz != 0)
                    //{
                    //    categoryList.Add("neurological");
                    //}
                    if (med == seiz && med != 0)
                    {
                        categoryList.Add("medication");
                    }
                    //if (med == nr && med != 0)
                    //{
                    //    categoryList.Add("neurological");
                    //}
                    //if (med == dt && med != 0)
                    //{
                    //    categoryList.Add("diet");
                    //}

                }
                Console.WriteLine(med);
                Console.WriteLine( seiz);
                Console.WriteLine(chr);
                Console.WriteLine(nr);
                Console.WriteLine(dt);
                Console.WriteLine(srg);
                Console.WriteLine( psych);
                Console.WriteLine( "_______________________________________________________");
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
