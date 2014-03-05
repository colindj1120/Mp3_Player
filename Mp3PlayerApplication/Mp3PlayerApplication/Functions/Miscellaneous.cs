using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Mp3.Functions
{
    class Miscellaneous
    {
        public static bool checkNull(DataGridViewRow row)
        {
            bool isNull = false;
            if (row.Cells[0].Value == null && row.Cells[1].Value == null && row.Cells[2].Value == null &&
                row.Cells[3].Value == null && row.Cells[4].Value == null && row.Cells[5].Value == null &&
                row.Cells[6].Value == null && row.Cells[7].Value == null && row.Cells[8].Value == null)
            {
                isNull = true;
            }
            return isNull;
        }
        public static void ProcessDirectory(string targetDirectory, List<string> listSongs)
        {
            // Process the list of files found in the directory. 
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
                listSongs.Add(fileName);

            // Recurse into subdirectories of this directory. 
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
                ProcessDirectory(subdirectory, listSongs);
        }

        public static int totalTime(String length)
        {
            string[] split = length.Split(':');
            int seconds = int.Parse(split[2]);
            int minutes = int.Parse(split[1]);
            int hours = int.Parse(split[0]);

            int total = (hours * 3600) + (minutes * 60) + seconds;

            return total;
        }
    }
}
