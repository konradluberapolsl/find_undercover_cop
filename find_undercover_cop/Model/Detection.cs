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
        private Mat Source = new Mat();
        private Dictionary<int, Rectangle> possibleContours = new Dictionary<int, Rectangle>();
        private List<List<Rectangle>> possibleAreas = new List<List<Rectangle>>();
        private List<Bitmap> letters = new List<Bitmap>();
        public List<Bitmap> Letters { get => letters; }
        public Detection(string path)
        {
            Source = CvInvoke.Imread(path, Emgu.CV.CvEnum.ImreadModes.AnyColor);
            FirstStep(PreProces(Source));
            SecondStep();
        }

        public Mat PreProces(Mat src)
        {
            Mat gray = new Mat();
            Mat bin = new Mat();
            CvInvoke.CvtColor(src, gray, ColorConversion.Bgr2Gray);
            CvInvoke.AdaptiveThreshold(gray, bin, 255, AdaptiveThresholdType.GaussianC, ThresholdType.Binary, 55, 5);
            return bin;
        }
        public void FirstStep(Mat bin)
        {
            Emgu.CV.Util.VectorOfVectorOfPoint contours = new Emgu.CV.Util.VectorOfVectorOfPoint();
            Mat hierarchy = new Mat();
            CvInvoke.FindContours(bin, contours, hierarchy, Emgu.CV.CvEnum.RetrType.Ccomp, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);
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

        public void SecondStep()
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
                    CvInvoke.Rectangle(Source, r, new MCvScalar(0, 0, 255), 0);
                    var image = Source.ToImage<Bgr, Byte>();
                    image.ROI = r;
                    var out_img = image.Copy();
                    possibleplates.Add(out_img);
                    /*                        string out_path_plate = @".\plates\" + file.Replace(@".\in\", n.ToString());
                                            CvInvoke.Imwrite(out_path_plate, out_img);*/
                }
                last = r;
            }
            foreach (var item in possibleplates)
            {
                Image<Gray, Byte> g = item.Convert<Gray, Byte>();
                Mat b = new Mat();
                CvInvoke.AdaptiveThreshold(g, b, 255, AdaptiveThresholdType.GaussianC, ThresholdType.Binary, 55, 5);
                Emgu.CV.Util.VectorOfVectorOfPoint con = new Emgu.CV.Util.VectorOfVectorOfPoint();
                Mat hi = new Mat();
                CvInvoke.FindContours(b, con, hi, Emgu.CV.CvEnum.RetrType.Ccomp, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);
                // CvInvoke.DrawContours(item, con, -1, new MCvScalar(255, 0, 0), 1);
                for (int i = 0; i < con.Size; i++)
                {
                    Rectangle brect = CvInvoke.BoundingRectangle(con[i]);
                    double ar = (double)brect.Width / (double)brect.Height;
                    if (ar > 4.2 && ar < 5.5 && brect.Width > 200)
                    {

                        CvInvoke.Rectangle(item, brect, new MCvScalar(0, 0, 255), 0);
                        var image = item;
                        image.ROI = brect;
                        var out_img = image.Copy();
                        Image<Gray, Byte> gray_plate = out_img.Convert<Gray, Byte>();
                        Emgu.CV.Util.VectorOfVectorOfPoint plate_contours = new Emgu.CV.Util.VectorOfVectorOfPoint();
                        Mat plate_hierarchy = new Mat();
                        Mat binary_plate = new Mat();
                        CvInvoke.AdaptiveThreshold(gray_plate, binary_plate, 255, AdaptiveThresholdType.GaussianC, ThresholdType.Binary, 55, 5);
                        CvInvoke.FindContours(binary_plate, plate_contours, plate_hierarchy, Emgu.CV.CvEnum.RetrType.Ccomp, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);
                        //CvInvoke.DrawContours(out_img, plate_contours, -1, new MCvScalar(255, 0, 0), 1);
                        for (int j = 0; j < plate_contours.Size; j++)
                        {

                            Rectangle br = CvInvoke.BoundingRectangle(plate_contours[j]);
                            double a = (double)br.Width / (double)br.Height;
                            if (a < 0.8 && br.Height < 60 && br.Height > 25)
                            {
                                CvInvoke.Rectangle(out_img, br, new MCvScalar(0, 0, 255), 0);
                                var letter = out_img;
                                letter.ROI = br;
                                var out_letter = letter.Copy();
                                Image<Gray, Byte> gray_letter = out_letter.Convert<Gray, Byte>();
                                Image<Gray, byte> binary_letter = new Image<Gray, byte>(gray_letter.Width,gray_letter.Height,new Gray(0));
                                CvInvoke.AdaptiveThreshold(gray_letter, binary_letter, 255, AdaptiveThresholdType.GaussianC, ThresholdType.Binary, 55, 5);
                                Image<Bgr, byte> o = binary_letter.Convert<Bgr, byte>();
                                //letters.Add(binary_letter.ToBitmap<Gray, byte>());
                                letters.Add(o.ToBitmap<Bgr,byte>());
                            }
                        }
                    }
                }
            }
        }


        //public void DeSkew()
        //{
        //    Image<Gray, byte> image = new Image<Gray, byte>(pPth);
        //    Gray cannyThreshold = new Gray(180);
        //    Gray cannyThresholdLinking = new Gray(120);
        //    Gray circleAccumulatorThreshold = new Gray(500);
        //    Image<Gray, Byte> cannyEdges = image.Canny(120,500);
        //    LineSegment2D[] lines = cannyEdges.HoughLinesBinary(
        //    1, //Distance resolution in pixel-related units
        //    Math.PI / 45.0, //Angle resolution measured in radians. ******
        //    100, //threshold
        //    30, //min Line width
        //    10 //gap between lines
        //    )[0]; //Get the lines from the first channel
        //    double[] angle = new double[lines.Length];

        //    for (int i = 0; i < lines.Length; i++)
        //    {

        //        double result = (double)((lines.P2.Y - lines.P1.Y) / (lines.P2.X - lines.P1.X));
        //        angle = Math.Atan(result) * 57.2957795;

        //    }
        //    double avg = 0;
        //    for (int i = 0; i < lines.Length; i++)
        //    {
        //        avg += angle[i];
        //    }
        //    avg = avg / lines.Length;
        //    Gray g = new Gray(255);
        //    Image<Gray, byte> imageRotate = image.Rotate(-avg, g);
        //}
    }
}
