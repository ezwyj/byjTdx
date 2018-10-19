using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Core.Entity;
using Tesseract;

namespace Core
{
    public  class MainHelper
    {
        private bool Debug = false;

        private List<Entity.MonitorLocation> _monitorList;
        public static List<TesseractEngine> OcrEngine { get; set; }
        private static List<ConcurrentQueue<Bitmap>> OcrImage = new List<ConcurrentQueue<Bitmap>>();
        private IntPtr DZHMainHandler;
        private IntPtr hbitmap, hmemdc;
        /// <summary>retList
        /// 读取配置文件，初始化全局变量
        /// </summary>
        public MainHelper(string configJson,bool isDebug)
        {
            Debug = isDebug;
            _monitorList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<MonitorLocation>>(configJson);
            OcrEngine = new List<TesseractEngine>(_monitorList.Count);
           
            var ProcessList = Process.GetProcesses();
            var DzhProcessId = 0;
            foreach (var item in ProcessList)
            {
                if (item.MainWindowTitle.IndexOf("大智慧") >= 0 && item.ProcessName == "dzh2")
                {
                    DzhProcessId = item.Id;
                    DZHMainHandler = item.MainWindowHandle;
                    break;
                }
            }
            Win32Api.SetForegroundWindow(DZHMainHandler);
            IntPtr hscrdc = Win32Api.GetWindowDC(DZHMainHandler);
            Win32Api.RECT windowRect = new Win32Api.RECT();
            Win32Api.GetWindowRect(DZHMainHandler, ref windowRect);
            int width = Convert.ToInt16( Math.Abs(windowRect.Right - windowRect.Left) * 1.25);
            int height = Convert.ToInt32(Math.Abs(windowRect.Bottom - windowRect.Top) * 1.25); ;
            hbitmap = Win32Api.CreateCompatibleBitmap(hscrdc, width, height);
            hmemdc = Win32Api.CreateCompatibleDC(hscrdc);
            Win32Api.SelectObject(hmemdc, hbitmap);
            Win32Api.PrintWindow(DZHMainHandler, hmemdc, 0);
            Win32Api.DeleteDC(hmemdc);


        }
        

        public void Run(int IntervalTime)
        {
            while (1 == 1)
            {
                var appImage = Screenshot();

                System.Threading.Thread.Sleep(IntervalTime);
            }
        }
        
        /// <summary>
        /// 图
        /// </summary>
        public Bitmap Screenshot()
        {
           
            Win32Api.SetForegroundWindow(DZHMainHandler);
           
            Bitmap bmp = Image.FromHbitmap(hbitmap);
            
            if (Debug)
            {
                var fileName = System.AppDomain.CurrentDomain.BaseDirectory+ "MinaImg"+DateTime.Now.ToString("yyyyMMddhhmmssfff")+".png";
                bmp.Save(fileName);
            }
            return bmp;
        }
        /// <summary>
        /// 切图
        /// </summary>
        public void CatImage(Bitmap bmp, int i)
        {
            System.Drawing.Rectangle rectangle = new Rectangle();
            rectangle.X = Convert.ToInt16( _monitorList[i].X*1.25);
            rectangle.Y = Convert.ToInt16(_monitorList[i].Y*1.25);
            rectangle.Width = _monitorList[i].Width;
            rectangle.Height = _monitorList[i].Height;
            Bitmap catedImage = bmp.Clone(rectangle, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            if(Debug)
            {
                var fileName = System.AppDomain.CurrentDomain.BaseDirectory + "catImg1" + DateTime.Now.ToString("yyyyMMddhhmmssfff") + ".png";
                catedImage.Save(fileName);

            }
            
        }


        /// <summary>
        /// 并行ocr
        /// </summary>
        public void Ocr (Bitmap main,int i)
        {

        }
    }
}
