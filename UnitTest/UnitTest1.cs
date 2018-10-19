using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using Core;
using Core.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        
        [TestMethod]
        public void TestDebug()
        {
            MainHelper testClass = null;
            var one = new MonitorLocation();
            one.X = 1216;
            one.Y = 311;
            one.Width = 45;
            one.Height = 18;
            one.Name = "买一";

            var two =  new MonitorLocation();
            two.X = 1105;
            two.Y = 88;
            two.Height = 23;
            two.Width = 79;
            two.Name = "股票名称";

            System.Collections.Generic.List<MonitorLocation> list = new System.Collections.Generic.List<MonitorLocation>();
            list.Add(one);
            list.Add(two);

            testClass = new MainHelper(Newtonsoft.Json.JsonConvert.SerializeObject(list).ToString(), true);

            Stopwatch sw = new Stopwatch();
            sw.Start();
            var bmp = testClass.Screenshot();
            sw.Stop();
            Console.WriteLine("截图花费："+ sw.ElapsedMilliseconds.ToString());
            sw.Reset();
            sw.Start();
            Parallel.Invoke(() => testClass.CatImage((Bitmap)bmp.Clone(), 0), () => testClass.CatImage((Bitmap)bmp.Clone(), 1));
            //testClass.CatImage(bmp, 0);
            //testClass.CatImage(bmp, 1);
            sw.Stop();
            Console.WriteLine("并行切图花费：" + sw.ElapsedMilliseconds.ToString());
        }

        [TestMethod]
        public void TestNoDebug()
        {

            MainHelper testClass = null;
            var one = new MonitorLocation();
            one.X = 1216;
            one.Y = 311;
            one.Width = 45;
            one.Height = 18;
            one.Name = "买一";

            var two = new MonitorLocation();
            two.X = 1119;
            two.Y = 88;
            two.Height = 77;
            two.Width = 24;
            two.Name = "股票名称";

            System.Collections.Generic.List<MonitorLocation> list = new System.Collections.Generic.List<MonitorLocation>();
            list.Add(one);
            list.Add(two);

            testClass = new MainHelper(Newtonsoft.Json.JsonConvert.SerializeObject(list).ToString(), false);

            Stopwatch sw = new Stopwatch();
            sw.Start();
            var bmp = testClass.Screenshot();
            sw.Stop();
            Console.WriteLine("截图花费：" + sw.ElapsedMilliseconds.ToString());
            sw.Reset();
            sw.Start();
            //Parallel.Invoke(() => testClass.CatImage(bmp, 0), () => testClass.CatImage(bmp, 1));
            testClass.CatImage(bmp, 0);
            sw.Stop();
            Console.WriteLine("并行切图花费：" + sw.ElapsedMilliseconds.ToString());

        }
    }
}
