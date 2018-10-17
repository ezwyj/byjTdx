using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tesseract;

namespace TdxAppMonitor
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            for(int i = 0; i < 10; i++)
            {
                GlobelVariable.OcrEngine[i] = new TesseractEngine("./tessdata", "eng", EngineMode.CubeOnly);
                GlobelVariable.OcrEngine[i].SetVariable("tessedit_char_whitelist", "0123456789");
            }

            Application.Run(new Form1());
        }
    }
}
