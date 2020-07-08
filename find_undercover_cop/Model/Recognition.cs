using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract;

namespace find_undercover_cop.Model
{
    class Recognition
    {
        private TesseractEngine ocr;
        private Page processed;
        private string text;
        public string Text
        {
            get => text;
            private set { text = value; }
        }
        public Recognition(Bitmap image)
        {
            Bitmap new_dpi = new Bitmap(image);
            new_dpi.SetResolution(300, 300);
            ocr = new TesseractEngine("./tessdata", "eng", EngineMode.TesseractAndCube);
            Read(image);
        }

        private void Read(Bitmap image)
        {
            processed = ocr.Process(image);
            Text = processed.GetText().Replace(" ", "").Replace("\n","");
        }
    }
}
