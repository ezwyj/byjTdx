using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


namespace ConsoleApp1
{
    class Program
    {
       

        static void Main(string[] args)
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            //DateTime start = DateTime.Now;

            //IntPtr ptr = (IntPtr)0x20BA6;
            //Rect AppRect = new Rect();
            //GetWindowRect(ptr, ref AppRect);
            //Rectangle rect = new Rectangle(AppRect.Left, AppRect.Top, (AppRect.Right - AppRect.Left), (AppRect.Bottom - AppRect.Top));
            //Bitmap bmp = new Bitmap(rect.Width, rect.Height, PixelFormat.Format32bppArgb);
            //Graphics g = Graphics.FromImage(bmp);
            //g.CopyFromScreen(rect.Left, rect.Top, 0, 0, bmp.Size, CopyPixelOperation.SourceCopy);

            //// make sure temp directory is there or it will throw.



            //bmp.Save(@"c:\test.jpg", ImageFormat.Jpeg);
            //List<IntPtr> listWnd = new List<IntPtr>();
            //EnumChildWindows(ptr, new EnumWindowProc(delegate (IntPtr hwnd, IntPtr lParam)
            //{
            //    int length = GetTextBoxTextLength(hwnd);
            //    StringBuilder sb = new StringBuilder();
            //    GetWindowText(hwnd, sb, length + 1);
            //    Console.WriteLine(sb.ToString());
            //    return true;

            //}), (IntPtr)0);
            //var list = GetAllChildrenWindowHandles(ptr, 1000);
            //foreach(var item in list)
            //{
            //    Console.WriteLine(item);
            //    EnumChildWindows(item, new EnumWindowProc(delegate (IntPtr hwnd, IntPtr lParam)
            //    {
            //        int length = GetTextBoxTextLength(hwnd);
            //        StringBuilder sb = new StringBuilder();
            //        GetWindowText(hwnd, sb, length + 1);
            //        Console.WriteLine(sb.ToString());
            //        return true;

            //    }), (IntPtr)0);
            //}
            //var ocr = new TesseractEngine("./tessdata", "eng", EngineMode.CubeOnly);
            //ocr.SetVariable("tessedit_char_whitelist", "0123456789");

            //var img = new Bitmap(@"c:\bb.png");
            ////var img = CaptureScreen(1525, 187, 200, 20);

            //var page = ocr.Process(img);
            //var result = page.GetText();

            var ProcessList = Process.GetProcesses();
            var DzhProcessId = 0;
            foreach (var item in ProcessList)
            {
                if (item.MainWindowTitle.IndexOf("Notepad++") > 0 && item.ProcessName == "notepad++")
                {
                    DzhProcessId = item.Id;
                    break;
                }
            }

            var result = Process.GetProcessById(DzhProcessId).MainWindowHandle;
            var hWnd = result;
            IntPtr hscrdc = Win32Api.GetWindowDC(hWnd);
            Rectangle windowRect = new Rectangle();
            Win32Api.GetWindowRect(hWnd, ref windowRect);
            int width = Math.Abs(windowRect.X - windowRect.Width);
            int height = Math.Abs(windowRect.Y - windowRect.Height);
            IntPtr hbitmap = Win32Api.CreateCompatibleBitmap(hscrdc, width, height);
            IntPtr hmemdc = Win32Api.CreateCompatibleDC(hscrdc);
            DateTime start = DateTime.Now;
            Win32Api.SelectObject(hmemdc, hbitmap);
            Win32Api.PrintWindow(hWnd, hmemdc, 0);
            Bitmap bmp = Image.FromHbitmap(hbitmap);
            DateTime end = DateTime.Now;
            Win32Api.DeleteDC(hscrdc);//删除用过的对象
            Win32Api.DeleteDC(hmemdc);//删除用过的对象
            bmp.Save(@"c:\dd"+ DateTime.Now.ToString("yyyyMMddhhmmss") + ".jpg");

            Parallel.Invoke(Ocr(bmp, 0), Ocr(bmp, 1), Ocr(bmp, 2));
            

            Console.WriteLine("{0}", sw.ElapsedMilliseconds);
            Console.Read();
        }

        private static void Ocr(Bitmap bmp,int i)
        {

        }

        private static Bitmap CaptureScreen(double x, double y, double width, double height)
        {
            int ix = Convert.ToInt32(x);
            int iy = Convert.ToInt32(y);
            int iw = Convert.ToInt32(width);
            int ih = Convert.ToInt32(height);

            System.Drawing.Bitmap bitmap = new Bitmap(iw, ih);
            using (System.Drawing.Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.CopyFromScreen(ix, iy, 0, 0, new System.Drawing.Size(iw, ih));
                graphics.Save();
                bitmap.Save(@"C:\p" + DateTime.Now.ToShortDateString().Replace("/", "") + ".jpg", System.Drawing.Imaging.ImageFormat.Png);
            }
            
            return bitmap;
        }

        static List<IntPtr> GetAllChildrenWindowHandles(IntPtr hParent, int maxCount)
        {
            List<IntPtr> result = new List<IntPtr>();
            int ct = 0;
            IntPtr prevChild = IntPtr.Zero;
            IntPtr currChild = IntPtr.Zero;
            while (true && ct < maxCount)
            {
                currChild = FindWindowEx(hParent, prevChild, null, null);
                if (currChild == IntPtr.Zero) break;
                result.Add(currChild);
                prevChild = currChild;
                ++ct;
            }
            return result;
        }
        [DllImport("user32.dll")]
        public static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int nMaxCount);


     
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindow(string strClassName, string strWindowName);
        [DllImport("user32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        //IMPORTANT : LPARAM  must be a pointer (InterPtr) in VS2005, otherwise an exception will be thrown
        public static extern bool EnumChildWindows(IntPtr window, EnumWindowProc callback, IntPtr i);
        //the callback function for the EnumChildWindows
        public delegate bool EnumWindowProc(IntPtr hWnd, IntPtr parameter);


        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hwnd, ref Rect rectangle);


        [DllImport("user32.dll", SetLastError = true)]
        public static extern int SendMessage(IntPtr hWnd, int msg, int Param, System.Text.StringBuilder text);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);
        public struct Rect
        {
            public int Left { get; set; }
            public int Top { get; set; }
            public int Right { get; set; }
            public int Bottom { get; set; }
        }

        static string GetWindowCaption(IntPtr hwnd)
        {
            StringBuilder sb = new StringBuilder(256);
            GetWindowCaption(hwnd, sb, 256);
            return sb.ToString();
        }
        static int GetTextBoxTextLength(IntPtr hTextBox)
        {
            // helper for GetTextBoxText
            uint WM_GETTEXTLENGTH = 0x000E;
            int result = SendMessage4(hTextBox, WM_GETTEXTLENGTH,
              0, 0);
            return result;
        }

        static string GetTextBoxText(IntPtr hTextBox)
        {
            uint WM_GETTEXT = 0x000D;
            int len = GetTextBoxTextLength(hTextBox);
            if (len <= 0) return null;  // no text
            StringBuilder sb = new StringBuilder(len + 1);
            SendMessage3(hTextBox, WM_GETTEXT, len + 1, sb);
            return sb.ToString();
        }

        [DllImport("user32.dll", EntryPoint = "GetWindowText",
  CharSet = CharSet.Auto)]
        static extern IntPtr GetWindowCaption(IntPtr hwnd,
  StringBuilder lpString, int maxCount);

        [DllImport("user32.dll", EntryPoint = "SendMessage",
  CharSet = CharSet.Auto)]
        static extern int SendMessage3(IntPtr hwndControl, uint Msg,
  int wParam, StringBuilder strBuffer); // get text

        [DllImport("user32.dll", EntryPoint = "SendMessage",
          CharSet = CharSet.Auto)]
        static extern int SendMessage4(IntPtr hwndControl, uint Msg,
          int wParam, int lParam);  // text length
    }
}
