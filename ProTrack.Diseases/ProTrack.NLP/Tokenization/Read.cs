using System;
using System.IO;

namespace ProTrack.NLP.Tokenization
{
    public class Read
    {
        private readonly string[] _pathFolder = Directory.GetFiles(@"F:\Master\Dizertatie\Work\RESULTS\dev", "1.txt");
       
        public void ReadFromFile()
        {
            foreach (var txtName in _pathFolder)
            {
                var read = File.ReadAllText(txtName);
                if (read.Contains(FileEntity.Entity.Background  ))
                {
                    File.Copy(txtName, _destinationFolder + Path.GetFileName(txtName), true);
                    Console.WriteLine("***#####" + txtName + "#####***");
                    Console.WriteLine(read);
                }
            }
        }
    }
}
