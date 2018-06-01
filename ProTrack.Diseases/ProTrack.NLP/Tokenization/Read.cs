using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ProTrack.NLP.Tokenization
{
    public static class Read
    {
       // private static readonly string[] PathFolder = Directory.GetFiles(@"F:\Master\Dizertatie\Work\RESULTS\dev", "1.txt");
        private static readonly string[] PathFolder = Directory.GetFiles(@"F:\Master\Dizertatie\Work\RESULTS\dev1", "*.txt");

        public static List<string> ReadFromFile()
        {
            var read = new List<string>();
            foreach (var txtName in PathFolder)
            {
                var text = File.ReadAllText(txtName);
                read.Add(text);
            }
            return read;
        }

        public static List<string> ReadFirstLine()
        {
            var read = new List<string>();
            foreach (var txtName in PathFolder)
            {
                var text = File.ReadLines(txtName).Skip(1).Take(1).ToList();
                var value = text.FirstOrDefault();
                read.Add(value);
            }
            return read;
        }
    }
}
