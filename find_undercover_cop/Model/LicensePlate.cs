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

        //czy jest gliniarzem
        public bool isUndercoverCop { get; set; }

        #endregion

        #region Ctor

        public LicensePlate(string fullLicensePlate)
        {
            FullLicensePlate = fullLicensePlate;

            LocationShortcut = FullLicensePlate.Substring(0, 3).Trim();
            LocationFullName = LocationShortcutToLocationFullName(LocationShortcut);
            LocationVoivodeship = LocationShortcutToLocationVoivodeship(LocationShortcut);

            RandomCharacters = FullLicensePlate.Substring(3).Trim();

            isUndercoverCop = CheckIfItsCop(FullLicensePlate);
        }

        #endregion

        #region Methods

        public string LocationShortcutToLocationFullName(string shortcut)
        {
            string fullName = null;
            string path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\database\list_of_markings.txt"));
            //plik -> tablica dwuwymiarowa o kolumnach shortcut,voivodeship,fullname
            string[][] listOfMarkings = ReadDatabase(path);
            for (int i = 0; i < listOfMarkings.Length; i++)
            {
                if (listOfMarkings[i][0] == shortcut)
                {
                    fullName = listOfMarkings[i][2];
                }
            }

            return fullName;
        }
        public string LocationShortcutToLocationVoivodeship(string shortcut)
        {
            string voivodeship = null;
            string path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\database\list_of_markings.txt"));
            //plik -> tablica dwuwymiarowa o kolumnach shortcut,voivodeship,fullname
            string[][] listOfMarkings = ReadDatabase(path);
            for (int i = 0; i < listOfMarkings.Length; i++)
            {
                if (listOfMarkings[i][0] == shortcut)
                {
                    voivodeship = listOfMarkings[i][1];
                }
            }

            return voivodeship;
        }
        public bool CheckIfItsCop(string licensePlate)
        {
            string path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\database\police_cars.txt"));
            string[][] policeCars = ReadDatabase(path);
            for (int i = 0; i < policeCars.Length; i++)
            {
                if (policeCars[i][0] == licensePlate)
                {
                    return true;
                }
            }
            return false;
        }
        public string GetCopCar(string licensePlate)
        {
            string copCar = null;
            string path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\database\police_cars.txt"));
            string[][] policeCars = ReadDatabase(path);
            for (int i = 0; i < policeCars.Length; i++)
            {
                if (policeCars[i][0] == licensePlate)
                {
                    copCar = policeCars[i][1];
                }
            }
            return copCar;
        }
        public string[][] ReadDatabase(string path)
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