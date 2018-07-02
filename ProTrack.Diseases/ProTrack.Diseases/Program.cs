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
            var write = new WriteCsv();

            //biomedical linked data
            var context = analyze.AnalyzeContext();
            var treatments = analyze.AnalyzeTreatments();
            var efficiency = analyze.AnalyzeEfficiency();
            var diseaseLvel = analyze.AnalyzeDiseaseLevel();
            var titles = Read.GetFilesTitle();
            //write.CreateCSV(context, treatments, efficiency, diseaseLvel, titles);

            //epilepsy categories
            var reprocessingDiseaseLevelList = analyze.ReprocessingDiseaseLevel(diseaseLvel);
            var reprocessingEfficiencyLevelList = analyze.ReprocessingEfficiancy();
            var reprocessingContextList = analyze.ReprocessingContext();
            //write.CreateCSV(reprocessingContextList, treatments, reprocessingEfficiencyLevelList, reprocessingDiseaseLevelList, titles);

            int i = 0;
            Console.WriteLine("___________________________________________________________________________________");
            Console.Write("___________________________________");
            Console.Write("ProTrack Tool");
            Console.Write("___________________________________");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            WriteFullLine("-----------------------------------------------------------------------------------", ConsoleColor.Cyan);
            WriteFullLine("----------------------------------------Menu---------------------------------------", ConsoleColor.Cyan);
            Console.WriteLine("                      Biomedical Linked Data File:  |1|");
            Console.WriteLine();
            Console.WriteLine("                      Epilepsy Categories File:     |2|");
            WriteFullLine("-----------------------------------------------------------------------------------", ConsoleColor.Cyan);
            WriteFullLine("-----------------------------------------------------------------------------------", ConsoleColor.Cyan);
            Console.WriteLine();
            Console.WriteLine();

            
            string option = "Yes";
            while (option == "Yes")
            {
                Console.WriteLine("***************************************");
                Console.WriteLine("Please enter a value: ");
                i = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("***************************************");
                switch (i)
                {
                    case 1:
                        Console.WriteLine();
                        Console.WriteLine("#######################################");
                        Console.WriteLine("Creating epilepsy categories file...");
                        write.CreateCSV(reprocessingContextList, treatments, reprocessingEfficiencyLevelList, reprocessingDiseaseLevelList, titles);
                        WriteFullLine("Created successfully", ConsoleColor.Green);
                        Console.WriteLine("#######################################");
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.WriteLine("If you want to continue write: |Yes|");
                        option = Console.ReadLine();
                        Console.WriteLine();
                        Console.WriteLine();
                        break;
                    case 2:
                        Console.WriteLine();
                        Console.WriteLine("#######################################");
                        Console.WriteLine("Creating linked data file...");
                        write.CreateCSV(context, treatments, efficiency, diseaseLvel, titles);
                        WriteFullLine("Created successfully", ConsoleColor.Green);
                        Console.WriteLine("#######################################");
                        Console.WriteLine();
                        Console.WriteLine("To continue write Yes");
                        option = Console.ReadLine();
                        Console.WriteLine();
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.WriteLine("Please try again");
                        Console.WriteLine();
                        Console.WriteLine();
                        break;
                }
            }
        }

        static void WriteFullLine(string value, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(value.PadRight(Console.WindowWidth - 1)); // <-- see note
            Console.ResetColor();
        }
    }
}
