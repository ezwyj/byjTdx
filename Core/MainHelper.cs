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
                if (item.MainWindowTitle.IndexOf("") > 0 && item.ProcessName == "dzh2")
                {
                    DzhProcessId = item.Id;
                    break;
                }
            }
            DZHMainHandler = Process.GetProcessById(DzhProcessId).MainWindowHandle;
            IntPtr hscrdc = Win32Api.GetWindowDC(DZHMainHandler);
            Rectangle windowRect = new Rectangle();
            Win32Api.GetWindowRect(DZHMainHandler, ref windowRect);
            int width = Math.Abs(windowRect.X - windowRect.Width);
            int height = Math.Abs(windowRect.Y - windowRect.Height);
            hbitmap = Win32Api.CreateCompatibleBitmap(hscrdc, width, height);
            hmemdc = Win32Api.CreateCompatibleDC(hscrdc);
        }
        

        public void Run(int IntervalTime)
        {
            while (1 == 1)
            {
                var appImage = Screenshot();
                MainImage.Enqueue(appImage);
                System.Threading.Thread.Sleep(IntervalTime);
            }
        }
        
        /// <summary>
        /// 图
        /// </summary>
        public Bitmap Screenshot()
        {
           
            Win32Api.SetForegroundWindow(DZHMainHandler);
            Win32Api.SelectObject(hmemdc, hbitmap);
            Win32Api.PrintWindow(DZHMainHandler, hmemdc, 0);
            Bitmap bmp = Image.FromHbitmap(hbitmap);
            
            if (Debug)
            {
                var fileName = System.AppDomain.CurrentDomain.BaseDirectory+ "MinaImg"+DateTime.Now.ToString("yyyyMMddhhmmssfff");
                bmp.Save(fileName);
            }
            return bmp;
        }
        /// <summary>
        /// 并行切图
        /// </summary>
        public void CatImage()
        {
            while (1 == 1)
            {
                Bitmap toDoImage;

                MainImage.TryDequeue(out toDoImage);
                
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
