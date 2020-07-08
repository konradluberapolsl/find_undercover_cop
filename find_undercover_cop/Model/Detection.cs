using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System.Drawing;
using Emgu.CV.Features2D;
using System.Threading.Tasks;

namespace find_undercover_cop.Model.AI
{
    class Detection
    {
        #region Fields  
        private Mat source = new Mat();
        private Dictionary<int, Rectangle> possibleContours = new Dictionary<int, Rectangle>();
        private List<List<Rectangle>> possibleAreas = new List<List<Rectangle>>();
        private Mat gray = new Mat();
        public Mat Gray
        {
            get => gray;
            private set
            {
                gray = value;
            }
        }
        private Image<Bgr, byte> outImage = null;
        public Image<Bgr,byte> OutImage
        {
            get => outImage;
            private set
            {
                outImage = value;
            }
        }

        private Mat binary =  new Mat();
        public Mat Binary
        {
            get => binary;
            private set
            {
                binary = value;
            }
        }

        private Mat middle = new Mat();
        public Mat Middle
        {
            get => middle;
            private set
            {
                middle = value;
            }
        }
        #endregion

        #region Ctor
        public Detection(string path)
        {
            source = CvInvoke.Imread(path, Emgu.CV.CvEnum.ImreadModes.AnyColor);
            ConvertToGrey();
            ConvertToBinary();
            FindPossibleAreas();
            FindROI();
            DeSkew();
        }
        #endregion

        #region Methods
        private void ConvertToGrey()
        {
            CvInvoke.CvtColor(source, gray, ColorConversion.Bgr2Gray);
        }

        private void Denoise()
        {
            CvInvoke.MedianBlur(Gray, Middle, 3);
            //CvInvoke.GaussianBlur(Gray, Middle, new Size(5, 5), 1.5);
        }


        private void ConvertToBinary()
        {
            //CvInvoke.Threshold(Middle, Binary, 50, 255, ThresholdType.Binary);
            CvInvoke.AdaptiveThreshold(Gray, Binary, 255, AdaptiveThresholdType.GaussianC, ThresholdType.Binary,55, 5);
        
        }


        private void FindPossibleAreas()
        {
            Emgu.CV.Util.VectorOfVectorOfPoint contours = new Emgu.CV.Util.VectorOfVectorOfPoint();
            Mat hierarchy = new Mat();
            CvInvoke.FindContours(Binary, contours, hierarchy, Emgu.CV.CvEnum.RetrType.Ccomp, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);
            for (int i = 0; i < contours.Size; i++)
            {
                Rectangle brect = CvInvoke.BoundingRectangle(contours[i]);
                double ar = (double)brect.Width / (double)brect.Height; 
                if (ar < 0.8 && brect.Height < 60 && brect.Height > 15)
                {
                    // CvInvoke.Rectangle(src, brect, new MCvScalar(0, 0, 255), 3);
                    var moments = CvInvoke.Moments(contours[i]);
                    int x = (int)(moments.M10 / moments.M00);
                    int y = (int)(moments.M01 / moments.M00);
                    //CvInvoke.PutText(src, (i + 1).ToString(), new Point(x+2, y+2), FontFace.HersheySimplex,0.5, new MCvScalar(255,0,0),1);
                    possibleContours.Add(i, brect);
                }
            }

            int index = -1;
            int density = 0;
            List<List<Rectangle>> list = new List<List<Rectangle>>();
            List<Rectangle> lst = new List<Rectangle>();
            foreach (var item in possibleContours)
            {

                if (index != -1)
                {
                    if (index == item.Key - 1)
                    {
                        if (!lst.Contains(possibleContours[index]))
                            lst.Add(possibleContours[index]);
                        density++;
                        lst.Add(item.Value);
                    }
                    else if (density > 1)
                    {
                        //Console.WriteLine(density);
                        density = 0;
                        possibleAreas.Add(lst);

                        lst = new List<Rectangle>();
                    }
                    else
                    {
                        density = 0;
                        lst = new List<Rectangle>();
                    }
                }
                index = item.Key;
            }
        }

        private void FindROI()
        {
            Rectangle last = new Rectangle();
            int n = 0;
            List<Image<Bgr, Byte>> possibleplates = new List<Image<Bgr, byte>>();
            foreach (var item in possibleAreas)
            {
                Console.WriteLine(item.Capacity);
                Rectangle r = new Rectangle(item[0].X - 200, item[0].Y - 50, 375, 150);
                if (!r.IntersectsWith(last))
                {
                    n++;
                    //CvInvoke.Rectangle(Source, r, new MCvScalar(0, 0, 255), 0);
                    var image = source.ToImage<Bgr, Byte>();
                    image.ROI = r;
                    OutImage = image.Copy();
                  
                }
                last = r;
            }
         
                
            
        }

        public Bitmap ConvertOutImgToBitmap()
        {
            Image<Bgr, byte> tmp = OutImage;
            //CvInvoke.FastNlMeansDenoising(OutImage, tmp);
            CvInvoke.MedianBlur(OutImage, tmp, 3);
            //CvInvoke.GaussianBlur(OutImage, tmp, new Size(5, 5), 1.5);

            return tmp.ToBitmap();
        }

        public void DeSkew()
        {
            Image<Gray, byte> image = OutImage.Convert<Gray, Byte>();
            //int height = tmp.Width;
            //int width = tmp.Height;

            //Image<Gray, byte> tmp_dn = null;
            //CvInvoke.FastNlMeansDenoising(tmp, tmp_dn);

            //Image<Gray, byte> tmp_bw = null;
            //CvInvoke.Threshold(tmp_dn, tmp_bw, 0, 255, ThresholdType.BinaryInv | ThresholdType.Otsu);

            //Image<Gray, byte> lines = null;
            //CvInvoke.HoughLines(tmp_bw, lines, width / 12, width / 150, 180); //sprawdzić trsh

            double cannyThreshold = 180;
            double cannyThresholdLinking = 120;
            Gray circleAccumulatorThreshold = new Gray(500);
            Image<Gray, Byte> cannyEdges = image.Canny(cannyThreshold, cannyThresholdLinking);
            LineSegment2D[] lines = cannyEdges.HoughLinesBinary(
            1, //Distance resolution in pixel-related units
            Math.PI / 180, //Angle resolution measured in radians. ******
            100, //threshold
            outImage.Width/12, //min Line width
            outImage.Width / 150 //gap between lines
            )[0]; //Get the lines from the first channel
            double[] angle = new double[lines.Length];

            for (int i = 0; i < lines.Length; i++)
            {
                double result = (double)(lines[i].P2.Y - lines[i].P1.Y) / (lines[i].P2.X - lines[i].P1.X);
                angle[i] = Math.Atan(result) * 57.2957795;
            }
            double avg = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                avg += angle[i];
            }
            avg /= lines.Length;
            Console.WriteLine(avg);
            //Gray g = new Gray(255);
            //Image<Gray, byte> imageRotate = image.Rotate(-avg, g);
            outImage = outImage.Rotate(-avg, new Bgr(0,0,0));
           

        }
        #endregion
    }
}
