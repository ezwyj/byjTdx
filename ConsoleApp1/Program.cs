using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tesseract;

namespace ConsoleApp1
{
    class Program
    {
        public static class AccessToken

        {
            // 调用getAccessToken()获取的 access_token建议根据expires_in 时间 设置缓存
            // 返回token示例
            public static String TOKEN = "24.adda70c11b9786206253ddb70affdc46.2592000.1493524354.282335-1234567";

            // 百度云中开通对应服务应用的 API Key 建议开通应用的时候多选服务
            private static String clientId = "nN5shGjIsPuVK0dN3cZP56rN";
            // 百度云中开通对应服务应用的 Secret Key
            private static String clientSecret = "pyik6Od7p1kgnL4BdX3MefAqjHM2SNHd";

            public static String getAccessToken()
            {
                String authHost = "https://aip.baidubce.com/oauth/2.0/token";
                HttpClient client = new HttpClient();
                List<KeyValuePair<String, String>> paraList = new List<KeyValuePair<string, string>>();
                paraList.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
                paraList.Add(new KeyValuePair<string, string>("client_id", clientId));
                paraList.Add(new KeyValuePair<string, string>("client_secret", clientSecret));

                HttpResponseMessage response = client.PostAsync(authHost, new FormUrlEncodedContent(paraList)).Result;
                String result = response.Content.ReadAsStringAsync().Result;
                TOKEN = result;
                Console.WriteLine(result);
                return result;
            }
        }
 

    static void Main(string[] args)
        {

            
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
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
            var fileList = new string[] { "A", "B", "C", "D" };
            var APP_ID = "14565074";
            var API_KEY = "nN5shGjIsPuVK0dN3cZP56rN";
            var SECRET_KEY = "pyik6Od7p1kgnL4BdX3MefAqjHM2SNHd";

            var client = new Baidu.Aip.Ocr.Ocr(API_KEY, SECRET_KEY);
            foreach (var fItem in fileList)
            {
                //var ocr = new TesseractEngine("./tessdata", "chi_sim", EngineMode.Default);
                //ocr.SetVariable("tessedit_char_whitelist", "委买卖一");

                var img = new Bitmap(@"C:\Saved Pictures\" + fItem + ".png");
                var image = File.ReadAllBytes(@"C:\Saved Pictures\" + fItem + ".png");
                // 调用通用文字识别, 图片参数为本地图片，可能会抛出网络等异常，请使用try/catch捕获
                var options = new Dictionary<string, object>{
                    {"recognize_granularity", "big"},
                    {"language_type", "CHN_ENG"},
                    {"detect_direction", "true"},
                    {"detect_language", "true"},
                    {"vertexes_location", "true"},
                    {"probability", "true"}
                };
                // 带参数调用通用文字识别（含位置信息版）, 图片参数为本地图片
                var result = client.General(image, options);
                var show = result["words_result"].ToList();
                var one = show.Where(a => a["words"].ToString().StartsWith("委买队列")).FirstOrDefault();
                Pen myPen = new Pen(System.Drawing.Color.BurlyWood, 3);

                var g = Graphics.FromImage(img);
                int width = Convert.ToUInt16( one["location"]["width"].ToString());
                int height = Convert.ToUInt16(one["location"]["height"].ToString());
                int x = Convert.ToUInt16(one["location"]["left"].ToString());
                int y = Convert.ToUInt16(one["location"]["top"].ToString());

                g.DrawRectangle(myPen, new Rectangle(x,y,width,height));
                Console.WriteLine(one);




                ////var img = CaptureScreen(1525, 187, 200, 20);
                //Pen myPen = new Pen(System.Drawing.Color.BurlyWood, 3);
                //var g = Graphics.FromImage(img);
                //var page = ocr.Process(img);
                //using (var iterator = page.GetIterator())
                //{

                //    iterator.Begin();
                //    do
                //    {
                //        string currentWord = iterator.GetText(PageIteratorLevel.Word);
                //        //do something with bounds 
                //        Tesseract.Rect bounds;
                //        iterator.TryGetBoundingBox(PageIteratorLevel.Word, out bounds);
                //        Console.WriteLine("x1" + bounds.X1.ToString());
                //        Console.WriteLine("Y1" + bounds.Y1.ToString());
                //        Console.WriteLine("x2" + bounds.X2.ToString());
                //        Console.WriteLine("y2" + bounds.Y2.ToString());
                //        Console.WriteLine(currentWord);
                //        //if (currentWord.IndexOf("委") > 0 && (currentWord.IndexOf("买") > 0 || currentWord.IndexOf("卖") > 0))
                //        //{
                //        //    g.DrawRectangle(myPen, new Rectangle(bounds.X1, bounds.Y1, bounds.X2, bounds.Y2));
                //        //}
                //        if (currentWord.IndexOf("一") > 0 && (currentWord.IndexOf("买") > 0 || currentWord.IndexOf("卖") > 0))
                //        {
                //            g.DrawRectangle(myPen, new Rectangle(bounds.X1, bounds.Y1, bounds.X2, bounds.Y2));
                //        }

                //    }
                //    while (iterator.Next(PageIteratorLevel.Word));
                //}


                Bitmap bmp = new Bitmap(img);
                img.Dispose();
                bmp.Save(@"C:\Saved Pictures\" + fItem + "_b.png"); //


            }
            

            //var result = page.GetText();
            //Console.WriteLine(result);

            //var ProcessList = Process.GetProcesses();
            //var DzhProcessId = 0;
            //foreach (var item in ProcessList)
            //{
            //    if (item.MainWindowTitle.IndexOf("Notepad++") > 0 && item.ProcessName == "notepad++")
            //    {
            //        DzhProcessId = item.Id;
            //        break;
            //    }
            //}

            //var result = Process.GetProcessById(DzhProcessId).MainWindowHandle;
            //var hWnd = result;
            //IntPtr hscrdc = Win32Api.GetWindowDC(hWnd);
            //Rectangle windowRect = new Rectangle();
            //Win32Api.GetWindowRect(hWnd, ref windowRect);
            //int width = Math.Abs(windowRect.X - windowRect.Width);
            //int height = Math.Abs(windowRect.Y - windowRect.Height);
            //IntPtr hbitmap = Win32Api.CreateCompatibleBitmap(hscrdc, width, height);
            //IntPtr hmemdc = Win32Api.CreateCompatibleDC(hscrdc);
            //DateTime start = DateTime.Now;
            //Win32Api.SelectObject(hmemdc, hbitmap);
            //Win32Api.PrintWindow(hWnd, hmemdc, 0);
            //Bitmap bmp = Image.FromHbitmap(hbitmap);
            //DateTime end = DateTime.Now;
            //Win32Api.DeleteDC(hscrdc);//删除用过的对象
            //Win32Api.DeleteDC(hmemdc);//删除用过的对象
            //bmp.Save(@"c:\dd"+ DateTime.Now.ToString("yyyyMMddhhmmss") + ".jpg");

            //Parallel.Invoke(Ocr(bmp, 0), Ocr(bmp, 1), Ocr(bmp, 2));

            sw.Stop();
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
