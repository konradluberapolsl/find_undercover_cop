﻿using System;
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

        public LicensePlate(string fullLicensePlate)
        {
            FullLicensePlate = fullLicensePlate;

            LocationShortcut = FullLicensePlate.Substring(0, 3).Trim();
            LocationFullName = LocationShortcutToLocationFullName(LocationShortcut);
            LocationVoivodeship = LocationShortcutToLocationVoivodeship(LocationShortcut);

            RandomCharacters = FullLicensePlate.Substring(3).Trim();

            isUnderCoverCop = false;
        }

        #endregion

        #region Methods

        public static string LocationShortcutToLocationFullName(string shortcut)
        {
            string fullName = null;
            string path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\database\list_of_markings.txt"));
            //plik -> tablica dwuwymiarowa o kolumnach shortcut,voivodeship,fullname
            string[][] listOfMarkings = ReadDatabaseOfNames(path);
            for (int i = 0; i < listOfMarkings.Length; i++)
            {
                if (listOfMarkings[i][0] == shortcut)
                {
                    fullName = listOfMarkings[i][2];
                }
            }

            return fullName;
        }
        public static string LocationShortcutToLocationVoivodeship(string shortcut)
        {
            string voivodeship = null;
            string path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\database\list_of_markings.txt"));
            //plik -> tablica dwuwymiarowa o kolumnach shortcut,voivodeship,fullname
            string[][] listOfMarkings = ReadDatabaseOfNames(path);
            for (int i = 0; i < listOfMarkings.Length; i++)
            {
                if (listOfMarkings[i][0] == shortcut)
                {
                    voivodeship = listOfMarkings[i][1];
                }
            }

            return voivodeship;
        }
        public static string[][] ReadDatabaseOfNames(string path)
        {
            string[] lines = File.ReadAllLines(path);
            string[][] data = new string[lines.Length][];
            for (int i = 0; i < lines.Length; i++)
            {
                string[] temp = lines[i].Split(',');
                data[i] = new string[temp.Length];
                for (int j = 0; j < temp.Length; j++)
                {
                    data[i][j] = temp[j];
                }
            }
            return data;
        }

        #endregion

    }
}
//