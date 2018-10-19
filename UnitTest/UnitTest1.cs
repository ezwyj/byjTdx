using System;
using System.Diagnostics;
using Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        MainHelper testClass = null;
        [TestMethod]
        public void TestInit()
        {
            testClass = new MainHelper("",true);
        }

        [TestMethod]
        public void TestScreenhot()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            testClass.Screenshot();
            sw.Stop();
            Trace.WriteLine("花费{0}", sw.ElapsedMilliseconds.ToString());

        }
    }
}
