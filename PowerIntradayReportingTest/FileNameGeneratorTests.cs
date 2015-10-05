using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PowerIntradayReporting;

namespace PowerIntradayReportingTest
{
    [TestClass]
    public class FileNameGeneratorTests
    {
        private IFileNameGenerator _generator;

        [TestInitialize]
        public void Init()
        {
            _generator = new FileNameGenerator();
        }

        [TestMethod]
        public void WillCreateTheReportFileNameBasedOnTheExtractTimeInTheDefinedFormat()
        {
            var extractTime = new DateTime(2015, 6, 12, 18, 36, 45);
            var fileName = _generator.Generate(extractTime);

            Assert.AreEqual("PowerPosition_20150612_1836.csv", fileName);
        }
    }
}