using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace find_undercover_cop.Model
{
    static class Converters
    {
        public static BitmapImage ToImageSource (Bitmap bitmap)  //Konwertuj Bitmape do ImageSource
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }

        public static string ConvertImageToBase64(Bitmap image) // Konwertuje Bitmape do strina Base64
        {
            using (MemoryStream memory = new MemoryStream())
            {
                image.Save(memory, ImageFormat.Jpeg);
                byte[] byteImage = memory.ToArray();
                return Convert.ToBase64String(byteImage);
            }
        }


    }
}
