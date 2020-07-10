using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Tesseract;

namespace find_undercover_cop.Model
{
    class Recognition
    {
        private TesseractEngine ocr; // Silnik Tesseract
        private Page processed; 
        private string text; //Wynikowy tekst
        public string Text
        {
            get => text;
            private set { text = value; }
        }


        public Recognition(Bitmap image)
        {
            Bitmap new_dpi = new Bitmap(image); 
            new_dpi.SetResolution(300, 300); // Zwiększenie dpi obrazu 
            ocr = new TesseractEngine("./tessdata", "eng", EngineMode.TesseractAndCube); //Inicajlizacja ocr
            Read(image);
        }

        private void Read(Bitmap image) //Odzczytanie znaków z obrazu
        {
            processed = ocr.Process(image);
            Text = processed.GetText();
            Regex pattern = new Regex(@"[^0-9a-zA-Z]+");
            Text = pattern.Replace(Text, "");
   
        }
    }
}
