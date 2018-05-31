using System;
using System.IO;
using ProTrack.AnalyzeFiles;
using ProTrack.NLP.NGrams;
using ProTrack.NLP.Stemming;
using ProTrack.NLP.Tokenization;

namespace ProTrack.Diseases
{
    class Program
    {
        //private static readonly string OneGram = File.ReadAllText(@"F:\Master\Dizertatie\Work\N-grams\1-gram.txt");
        //private static readonly string TwoGram = File.ReadAllText(@"F:\Master\Dizertatie\Work\N-grams\2-gram.txt");
        //private static readonly string Treatments = File.ReadAllText(@"F:\Master\Dizertatie\Work\N-grams\treatment.txt");

        static void Main(string[] args)
        {
            var split = new Split();
            //Console.WriteLine(split.SplitFile());
            //Console.WriteLine(ReadGrams.ReadNGram(OneGram));
            //Console.WriteLine(ReadGrams.ReadNGram(TwoGram));
           // var wordStem = new WordStem();
           // wordStem.FindGramStem(TwoGram);
           // ReadGrams.ReadNGram(Treatments);
            var analyze = new Analyze();
           // Console.WriteLine(analyze.AnalyzeContext());
            Console.WriteLine(analyze.AnalyzeTreatments());
        }
    }
}
