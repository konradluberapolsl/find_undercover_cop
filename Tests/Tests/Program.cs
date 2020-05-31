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
            Mat src = CvInvoke.Imread(path, Emgu.CV.CvEnum.ImreadModes.AnyColor);
            Mat gray = new Mat();
            Mat bin = new Mat();
            CvInvoke.CvtColor(src, gray, ColorConversion.Bgr2Gray);
            CvInvoke.Threshold(dst, bin, 100, 255, ThresholdType.Binary);
            CvInvoke.Imwrite(out_path_bin, bin);
        }
    }
}
