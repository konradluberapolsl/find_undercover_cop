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

        //pierwsza czesc tablicy
        public string LocationShortcut { get; set; }
        //miasto lub powiat
        public string LocationFullName { get; set; }
        //wojewodztwo
        public string LocationVoivodeship { get; set; }

        //druga czesc tablicy
        public string RandomCharacters { get; set; }

        //czy jest psem
        public bool isUnderCoverCop { get; set; }

        #endregion

        #region Ctor

        #endregion

        #region Methods

        public void ShortcutToFullName(string shortcut)
        {
            //plik z rozwinieciami
            string path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\database\list_of_markings.txt"));
        }

        #endregion

    }
}
