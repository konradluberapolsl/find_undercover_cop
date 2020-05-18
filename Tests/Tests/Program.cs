using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.UI;

namespace Tests
{
    class Program
    {
        
        static void Main(string[] args)
        {
            int threshold_value = 50; //0-255
            Mat src = CvInvoke.Imread(@"nowy.jpg", Emgu.CV.CvEnum.ImreadModes.AnyColor);
            Mat mid = new Mat();
            Mat gray = new Mat();
            Mat dst = new Mat();
            CvInvoke.CvtColor(src, gray, ColorConversion.Rgb2Gray);
            CvInvoke.MedianBlur(gray, mid, 3);
            CvInvoke.Canny(mid, dst, 100, 200);
            //Edge detection


            
            ////Bin with mean
            //CvInvoke.CvtColor(src, gray, ColorConversion.Bgr2Gray);
            //CvInvoke.AdaptiveThreshold(gray, dst, 255, Emgu.CV.CvEnum.AdaptiveThresholdType.MeanC, Emgu.CV.CvEnum.ThresholdType.Binary, 11, 2);

            //Operation on Image
            //Image<Bgr, Byte> dst = src.ToImage<Bgr, Byte>();
            //Image<Gray, byte> grayImage = dst.Convert<Gray, byte>();
            //CvInvoke.MedianBlur(grayImage, dst, 3);
            //Image<Gray, Byte> Binary_Image = grayImage.ThresholdBinary(new Gray(threshold_value), new Gray(255));
            CvInvoke.Imwrite(@"img2.jpg", dst);
            Console.ReadKey();
        }
    }
}
