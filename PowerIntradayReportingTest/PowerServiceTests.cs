using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services;

namespace PowerIntradayReportingTest
{
    [TestClass]
    public class PowerServiceTests
    {
        [TestMethod]
        public void Explore()
        {
            var service = new PowerService();
            var trades = service.GetTrades(new DateTime(2015, 10, 5, 23, 0, 0));
        }
    }
}