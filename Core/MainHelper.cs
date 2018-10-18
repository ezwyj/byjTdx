using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Core.Entity;
using Newtonsoft;
using Tesseract;

namespace Core
{
    public  class MainHelper
    {
        private bool Debug = false;

        private List<Entity.MonitorLocation> _monitorList;
        public static List<TesseractEngine> OcrEngine { get; set; }

        private IntPtr DZHMainHandler;
        /// <summary>retList
        /// 读取配置文件，初始化全局变量
        /// </summary>
        public MainHelper(string configJson,bool isDebug)
        {
            Debug = isDebug;
            _monitorList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<MonitorLocation>>(configJson);
            OcrEngine = new List<TesseractEngine>(_monitorList.Count);
        }
        #region  获取窗口句柄
        //private IntPtr GetHandleByProcessName(string programName)
        //{
        //    List<ProcessEntry32> list = new List<ProcessEntry32>();
        //    IntPtr handle = Win32Api.CreateToolhelp32Snapshot(0x2, 0);
        //    IntPtr hh = IntPtr.Zero;
        //    if ((int)handle > 0)
        //    {
        //        ProcessEntry32 pe32 = new ProcessEntry32();
        //        pe32.dwSize = (uint)Marshal.SizeOf(pe32);
        //        int bMore = Win32Api.Process32First(handle, ref pe32);
        //        while (bMore == 1)
        //        {
        //            IntPtr temp = Marshal.AllocHGlobal((int)pe32.dwSize);
        //            Marshal.StructureToPtr(pe32, temp, true);
        //            ProcessEntry32 pe = (ProcessEntry32)Marshal.PtrToStructure(temp, typeof(ProcessEntry32));
        //            Marshal.FreeHGlobal(temp);
        //            list.Add(pe);
        //            if (pe.szExeFile == programName)
        //            {
        //                bMore = 2;
        //                hh = GetCurrentWindowHandle(pe.th32ProcessID);
        //                break;
        //            }
        //            bMore = Win32Api.Process32Next(handle, ref pe32);
        //        }
        //    }
        //    return hh;
          
        //}
        //public  IntPtr GetCurrentWindowHandle(uint proid)
        //{
        //    IntPtr ptrWnd = IntPtr.Zero;
        //    uint uiPid = proid;
        //    object objWnd = processWnd[uiPid];
        //    if (objWnd != null)
        //    {
        //        ptrWnd = (IntPtr)objWnd;
        //        if (ptrWnd != IntPtr.Zero && IsWindow(ptrWnd))  // 从缓存中获取句柄
        //        {
        //            return ptrWnd;
        //        }
        //        else
        //        {
        //            ptrWnd = IntPtr.Zero;
        //        }
        //    }
        //    bool bResult = Win32Api.EnumWindows(new WNDENUMPROC(EnumWindowsProc), uiPid);
        //    // 枚举窗体返回 false 而且没有错误号时表明获取成功
        //    if (!bResult && Marshal.GetLastWin32Error() == 0)
        //    {
        //        objWnd = processWnd[uiPid];
        //        if (objWnd != null)
        //        {
        //            ptrWnd = (IntPtr)objWnd;
        //        }
        //    }
        //    return ptrWnd;
        //}


        //private static bool EnumWindowsProc(IntPtr hwnd, uint lParam)
        //{
        //    uint uiPid = 0;
        //    if (GetParent(hwnd) == IntPtr.Zero)
        //    {
        //        GetWindowThreadProcessId(hwnd, ref uiPid);
        //        if (uiPid == lParam)    // 找到进程相应的主窗体句柄
        //        {
        //            processWnd.Add(uiPid, hwnd);   // 把句柄缓存起来
        //            SetLastError(0);    // 设置无错误
        //            return false;   // 返回 false 以终止枚举窗体
        //        }
        //    }
        //    return true;
        //}

        #endregion
        private Bitmap GetImageFromWindow(IntPtr hWnd)
        {
            IntPtr hscrdc = Win32Api.GetWindowDC(hWnd);
            Rectangle windowRect = new Rectangle();
            Win32Api.GetWindowRect(hWnd, ref windowRect);
            int width = Math.Abs(windowRect.X - windowRect.Width);
            int height = Math.Abs(windowRect.Y - windowRect.Height);
            IntPtr hbitmap = Win32Api.CreateCompatibleBitmap(hscrdc, width, height);
            IntPtr hmemdc = Win32Api.CreateCompatibleDC(hscrdc);
            Win32Api.SelectObject(hmemdc, hbitmap);
            Win32Api.PrintWindow(hWnd, hmemdc, 0);
            Bitmap bmp = Image.FromHbitmap(hbitmap);
            Win32Api.DeleteDC(hscrdc);//删除用过的对象
            Win32Api.DeleteDC(hmemdc);//删除用过的对象
            return bmp;
           
        }
        /// <summary>
        /// //根据窗口名寻找控件
        /// </summary>
        public void Screenshot()
        {
            DateTime start = DateTime.Now;
          

            if (Debug)
            {
                DateTime end = DateTime.Now;
                TimeSpan sp = end - start;
                Console.Write(sp.TotalMilliseconds);
            }
        }
        /// <summary>
        /// 并行切图
        /// </summary>
        public void CatImage()
        {

        }

        /// <summary>
        /// 并行ocr
        /// </summary>
        public void Ocr ()
        {

        }
    }
}
