using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PowerIntradayReporting;
using PowerIntradayReporting.Model;

namespace PowerIntradayReportingTest
{
    [TestClass]
    public class ReportContentWriterTests
    {
        private IReportContentWriter _writer;

        [TestInitialize]
        public void Init()
        {
            _writer = new ReportContentWriter();
        }

        [TestMethod]
        public void WillWriteAStringRepresentingThePosition()
        {
            var extractTime = new DateTime(2015, 5, 12);
            var position = new PowerPosition(extractTime, 5);
            for (int i = 0; i < position.Periods.Length; i++)
            {                
                position.Periods[i].Volume = 10 * i;
            }

            var content = _writer.Write(position);

            Assert.AreEqual(Properties.Resources.ExpectedContent, content);
        }
    }
}