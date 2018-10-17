using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TdxAppMonitor
{
    public static class TDXAnalysis
    {


        private static Decimal ocrProcess(int i, Bitmap image)
        {
            var page = GlobelVariable.OcrEngine[i].Process(image);
            var result = page.GetText();
            return decimal.Parse(result);
        }
        public static Decimal GetBuyOne(Bitmap bigImage)
        {

            var image = ocrProcess(0,)


        }
    }
}
