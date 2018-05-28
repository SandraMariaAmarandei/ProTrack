﻿using System;
using System.Collections.Generic;
using System.IO;

namespace ProTrack.NLP.Tokenization
{
    public static class Read
    {
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
    }
}
