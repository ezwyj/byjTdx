using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract;

namespace TdxAppMonitor
{
    public static class GlobelVariable
    {
        static List<TesseractEngine>  _ocrEngine = new List<TesseractEngine>();
        public static List<TesseractEngine> OcrEngine { get; set; }
    }
}
