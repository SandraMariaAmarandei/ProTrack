using System.Collections.Generic;

namespace ProTrack.NLP.Tokenization
{
    public class Split
    {
        private string results = FileEntity.Entity.Results.ToString().ToUpper();
        private string conclusions = FileEntity.Entity.Conclusions.ToString().ToUpper();
        private string motivation = FileEntity.Entity.Methods.ToString().ToUpper();
        private string objective = FileEntity.Entity.Objective.ToString().ToUpper();
        private string background = FileEntity.Entity.Background.ToString().ToUpper();

        public List<RelationEntity> SplitFile()
        {
            var splitFile = new List<RelationEntity>();
            var textFiles = Read.ReadFromFile();
            foreach (var text in textFiles)
            {
                var i = 0;
                var file = new RelationEntity();
                string[] words = text.Split(' ',',','\n','\t','.');

                while (!words[i].Contains(results) && !words[i].Contains(conclusions))
                {
                    file.Cause = file.Cause + " " + words[i];
                    i++;
                }
                for (int j = i; j < words.Length; j++)
                {
                    file.Result = file.Result + " " + words[j];
                }
                splitFile.Add (file);
            }
            return splitFile;
        }

        public List<string> TakeMotivation()
        {
            var textFiles = Read.ReadFirstLine();
       
            return textFiles;
        }

        public List<string> TakeAllDocument()
        {
            return Read.ReadFromFile();
        }
    }
}
