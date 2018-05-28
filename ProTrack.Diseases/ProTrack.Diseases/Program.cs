using System;
using ProTrack.NLP.Tokenization;

namespace ProTrack.Diseases
{
    class Program
    {
        static void Main(string[] args)
        {
            var split = new Split();
            Console.WriteLine(split.SplitFile());
        }
    }
}
