using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace find_undercover_cop.Model
{
    static class Serialization
    {
        private static  string datapah = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\database\results.json"));

        public static void Save (ref LicensePlate licensePlate, string base64Image)
        {
            try
            {
                var jsonLicense = JsonConvert.SerializeObject(licensePlate) + " " + base64Image + "\n";
                File.AppendAllText(datapah, jsonLicense);
            }
            catch (Exception e)
            {

                
            }
        }
    }
}
