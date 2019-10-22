using Microsoft.VisualStudio.TestTools.UnitTesting;
using WirtualnyMagazyn.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WirtualnyMagazyn.Views.Tests
{
    [TestClass()]
    public class AwaitingTests
    {
        [TestMethod()]
        public void GetDateTest()
        {
            string data = Awaiting.GetDate();
            int expected = 8;
            int date = data.Length;

            Assert.AreEqual(expected, date);
        }
    }
}