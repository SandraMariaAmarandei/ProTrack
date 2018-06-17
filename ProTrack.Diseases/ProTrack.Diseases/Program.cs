using System;
using System.IO;
using ProTrack.AnalyzeFiles;
using ProTrack.Diseases.Results;
using ProTrack.NLP.NGrams;
using ProTrack.NLP.Stemming;
using ProTrack.NLP.Tokenization;

namespace ProTrack.Diseases
{
    class Program
    {
        static void Main(string[] args)
        {
            var analyze = new Analyze();
            //Console.WriteLine(analyze.AnalyzeContext());
            //Console.WriteLine(analyze.AnalyzeTreatments());
            // Console.WriteLine(analyze.AnalyzeEfficiency());
            //Console.WriteLine(analyze.EfficiencyEntitie());
            //Console.WriteLine(analyze.AnalyzeDiseaseLevel());
            var write = new WriteCsv();
           

            var context = analyze.AnalyzeContext();
            var treatments = analyze.AnalyzeTreatments();
            var efficiency = analyze.AnalyzeEfficiency();
            var diseaseLvel = analyze.AnalyzeDiseaseLevel();
            var titles = Read.GetFilesTitle();

            write.CreateCSV(context, treatments,efficiency, diseaseLvel, titles);
        }
    }
}
