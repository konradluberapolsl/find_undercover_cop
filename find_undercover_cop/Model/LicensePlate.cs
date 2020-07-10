using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Media;

namespace find_undercover_cop.Model
{
    public class LicensePlate
    {
        #region Properties

        private string fullLicensePlate;
        public string FullLicensePlate
        {
            get => fullLicensePlate;
            private set
            {
                fullLicensePlate = value;
            }
        }

        //pierwsza czesc tablicy rej
        private string locationShortcut;
        public string LocationShortcut 
        { get => locationShortcut;
          private set
          {
                locationShortcut = value;
          }
        }


        //miasto lub powiat
        private string locationFullName;
        public string LocationFullName
        {
            get =>  locationFullName;
            private set
            {
                locationFullName = value;
            }
        }
        //wojewodztwo
        private string locationVoivodeship;
        public string LocationVoivodeship
        {
            get => locationVoivodeship;
            private set
            {
                locationVoivodeship = value;
            }
        }

        //druga czesc tablicy rej
        private string randomCharacters;
        public string RandomCharacters
        {
            get => randomCharacters;
            private set
            {
                randomCharacters = value;
            }
        }

        //czy jest gliniarzem
        private bool isUndercoverCop;
        public bool IsUndercoverCop
        {
            get => isUndercoverCop;
            private set
            {
                isUndercoverCop = value;
            }
        }


        #endregion

        #region Ctor

        public LicensePlate(string fullLicensePlate)
        {
            
            FullLicensePlate = fullLicensePlate;
            GetLocatioShortcutAndRandomLetter();
            CheckIfItsCop();
            //Console.WriteLine(locationShortcut) ;
            //Console.WriteLine(LocationFullName);
            //Console.WriteLine(LocationVoivodeship);
            //Console.WriteLine(RandomCharacters);


        }

        #endregion

        #region Methods

        private void GetLocatioShortcutAndRandomLetter()
        {
            string tmp = fullLicensePlate.ToUpper();
            if (tmp.Length == 7)
            {
                string shortcut = tmp.Substring(0, 3);
                Console.WriteLine(tmp[2].ToString());
                if (!int.TryParse(shortcut, out _) && !int.TryParse(tmp[2].ToString(), out _))
                {
                    LocationShortcut = shortcut;
                    LocationFullName = LocationShortcutToLocationFullName(shortcut);
                    LocationVoivodeship = LocationShortcutToLocationVoivodeship(shortcut);
                    RandomCharacters = tmp.Substring(3);
                    return;
                }
                shortcut = tmp.Substring(0, 2);
                if (!int.TryParse(shortcut, out _))
                {
                    LocationShortcut = shortcut;
                    LocationFullName = LocationShortcutToLocationFullName(shortcut);
                    LocationVoivodeship = LocationShortcutToLocationVoivodeship(shortcut);
                    RandomCharacters = tmp.Substring(2);
                }
            }
            else if (tmp.Length == 8)
            {
                string shortcut = tmp.Substring(0, 3);
                Console.WriteLine(tmp[2].ToString());
                if (!int.TryParse(shortcut, out _) && !int.TryParse(tmp[2].ToString(), out _))
                {
                    LocationShortcut = shortcut;
                    LocationFullName = LocationShortcutToLocationFullName(shortcut);
                    LocationVoivodeship = LocationShortcutToLocationVoivodeship(shortcut);
                    RandomCharacters = tmp.Substring(3);
                    return;
                }
            }
        }
        



        private string LocationShortcutToLocationFullName(string shortcut)
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
        private string LocationShortcutToLocationVoivodeship(string shortcut)
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
        private void CheckIfItsCop()
        {
            string path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\database\police_cars.txt"));
            string[][] policeCars = ReadDatabase(path);
            for (int i = 0; i < policeCars.Length; i++)
            {
                if (policeCars[i][0] == fullLicensePlate)
                {
                    isUndercoverCop = true;
                }
            }
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
        private string[][] ReadDatabase(string path)
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