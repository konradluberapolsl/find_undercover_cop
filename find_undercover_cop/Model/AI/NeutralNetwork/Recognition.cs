using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace find_undercover_cop.Model.AI.NeutralNetwork
{
    public class Recognition
    {
        #region Fields

        public static int firstLetter = 'A';
        public static int lastLetter = 'Z';
        public static int firstDigit = '0';
        public static int lastDigit = '9';

        public static int letterCount = lastLetter - firstLetter + 1;
        public static int digitCount = lastDigit - firstDigit + 1;
        public static int charsCount = letterCount + digitCount;

        public static int aMatrixDim = 10;

        public PatternsCollection trainingPatterns;
        public OCRNetwork backpropNetwork;
        private readonly MainWindow owner;

        #endregion

        #region Ctor

        public Recognition()
        {
            //generate training patterns
            trainingPatterns = CreateTrainingPatternsFromDatabase();
            Console.WriteLine("-----------------------------GTP-----------------------------");

            //create the network object
            backpropNetwork = new OCRNetwork(owner, new int[3] { aMatrixDim * aMatrixDim, ((aMatrixDim * aMatrixDim) + charsCount) / 2, charsCount });
            Console.WriteLine("-----------------------------CNO-----------------------------");


            //now we should think if we want to train again or we load the network. 
            //but it's logic to load it
            backpropNetwork.Train(trainingPatterns);
            Console.WriteLine("-----------------------------TTN-----------------------------");
            //or
            //backpropNetwork.LoadFromFile(...........nazwapliku................);

            //save nn
            Console.WriteLine("-----------------------------SN-----------------------------");
            backpropNetwork.SaveToFile("nn.neuro");

        }

        #endregion

        #region Methods

        public PatternsCollection CreateTrainingPatternsFromDatabase()
        {
            var result = new PatternsCollection(charsCount, aMatrixDim * aMatrixDim, 1);

            //
            // litery
            //
            for (var i = 0; i < letterCount; i++)
            {
                string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\database\smpl\")) + Convert.ToChar(firstLetter + i) + "_binary\\0.jpg";
                Console.WriteLine(i + ". " + path);
                var aBitMatrix = BitmapToDoubleArray(new Bitmap(path), aMatrixDim);

                for (var j = 0; j < aMatrixDim * aMatrixDim; j++)
                {
                    result[i].Input[j] = aBitMatrix[j];
                }
                result[i].Output[0] = i;
            }
            //
            // cyfry
            //
            int t = 0;
            for (var i = letterCount; i < charsCount; i++)
            {
                string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\database\smpl\")) + Convert.ToChar(firstDigit + t) + "_binary\\0.png";
                Console.WriteLine(i + ". " + path);
                var aBitMatrix = BitmapToDoubleArray(new Bitmap(path), aMatrixDim);

                for (var j = 0; j < aMatrixDim * aMatrixDim; j++)
                {
                    result[i].Input[j] = aBitMatrix[j];
                }
                result[i].Output[0] = i;
                t++;
            }

            return result;
        }
        public double[] BitmapToDoubleArray(Bitmap aSrc, int aArrayDim)
        {
            var result = new double[aArrayDim * aArrayDim];

            Bitmap bt = new Bitmap(aSrc.Width, aSrc.Height);
            var bmp = Graphics.FromImage(bt);
            bmp.SmoothingMode = SmoothingMode.None;
            bmp.InterpolationMode = InterpolationMode.NearestNeighbor;

            var xStep = aSrc.Width / (double)aArrayDim;
            var yStep = aSrc.Height / (double)aArrayDim;
            for (var i = 0; i < aSrc.Width; i++)
                for (var j = 0; j < aSrc.Height; j++)
                {
                    var x = (int)((i / xStep));
                    var y = (int)(j / yStep);
                    var c = aSrc.GetPixel(i, j);
                    result[y * x + y] += Math.Sqrt(c.R * c.R + c.B * c.B + c.G * c.G);
                }
            return Scale(result);
        }
        private double[] Scale(double[] src)
        {
            var max = MaxOf(src);
            if (max != 0)
            {
                for (var i = 0; i < src.Length; i++)
                    src[i] = src[i] / max;
            }
            return src;
        }
        private double MaxOf(double[] src)
        {
            var res = double.NegativeInfinity;
            foreach (var d in src)
                if (d > res) res = d;
            return res;
        }
        public string Testing(double[] aInput)
        {
            var character = "";

            for (var i = 0; i < backpropNetwork.InputNodesCount; i++)
            {
                backpropNetwork.InputNode(i).Value = aInput[i];
            }
            backpropNetwork.Run();

            //
            // litera
            //
            if (backpropNetwork.BestNodeIndex < 26)
            {
                character = Convert.ToChar(firstLetter + backpropNetwork.BestNodeIndex).ToString();
            }
            //
            // cyfra
            //
            else
            {
                int t = backpropNetwork.BestNodeIndex - 26;
                character = Convert.ToChar(firstDigit + t).ToString();
            }

            return character;
        }


        //finish method which we will use to recognize in viewmodel
        public string Recognize(Bitmap bmp)
        {
            string s = null;

            //testing
            double[] aInput = BitmapToDoubleArray(bmp, aMatrixDim);
            s = Testing(aInput);

            return s;
        }

        #endregion
    }
}
