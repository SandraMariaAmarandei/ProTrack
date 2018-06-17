using Spire.Xls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;

namespace ProTrack.Diseases.Results
{
    public class WriteCsv
    {
        public void CreateCSV(List<string> context, List<string> treatments, List<string> efficiency, List<string> diseaseLevel, List<string> titles)
        {
            var filePath = Directory.GetFiles(@"F:\Master\Dizertatie\Work\RESULTS", "devdata.xlsx");

            Workbook wbToStream = new Workbook();
            Worksheet sheet = wbToStream.Worksheets[0];
            

            for (int i = 2; i < context.Count+2; i++)
            {
                sheet.Range["A" + i].Text = context.ElementAt(i - 2);
            }
            for (int i = 2; i < treatments.Count + 2; i++)
            {
                sheet.Range["B" + i].Text = treatments.ElementAt(i - 2);
            }
            for (int i = 2; i < efficiency.Count + 2; i++)
            {
                sheet.Range["C" + i].Text = efficiency.ElementAt(i - 2);
            }
            for (int i = 2; i < diseaseLevel.Count + 2; i++)
            {
                sheet.Range["D" + i].Text = diseaseLevel.ElementAt(i - 2);
            }
            for (int i = 2; i < titles.Count + 2; i++)
            {
                sheet.Range["E" + i].Text = titles.ElementAt(i - 2);
            }
            FileStream file_stream = new FileStream("To_stream.xls", FileMode.Create);
            wbToStream.SaveToStream(file_stream);
            file_stream.Close();
            System.Diagnostics.Process.Start("To_stream.xls");   
        }

    }
}
