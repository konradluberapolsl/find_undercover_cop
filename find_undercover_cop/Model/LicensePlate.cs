using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace find_undercover_cop.Model
{
    public class LicensePlate
    {
        #region Properties

        public string FullLicensePlate { get; set; }

        //pierwsza czesc tablicy rej
        public string LocationShortcut { get; set; }
        //miasto lub powiat
        public string LocationFullName { get; set; }
        //wojewodztwo
        public string LocationVoivodeship { get; set; }

        //druga czesc tablicy rej
        public string RandomCharacters { get; set; }

        //czy jest psem
        public bool isUnderCoverCop { get; set; }

        #endregion

        #region Ctor

        #endregion

        #region Methods

        public void ShortcutToFullName(string shortcut)
        {
            string fullName;
            //plik z rozwinieciami
            string path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\database\list_of_markings.txt"));
            //przerabianie pliku 
            string[] lines = File.ReadAllLines(path);
            string[][] listOfMarkings = new string[lines.Length][];
            for (int i = 0; i < lines.Length; i++)
            {
                string[] tmp = lines[i].Split(',');
                for (int j = 0; j < tmp.Length; j++)
                {
                    listOfMarkings[i][j] = tmp[j];
                }
            }
        }

        #endregion

    }
}
