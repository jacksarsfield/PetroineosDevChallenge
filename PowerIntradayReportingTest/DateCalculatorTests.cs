using System;
using Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PowerIntradayReporting;

namespace PowerIntradayReportingTest
{
    [TestClass]
    public class DateCalculatorTests
    {
        private IClock _clock;
        private IDateCalculator _dateCalculator;

        [TestInitialize]
        public void Initialise()
        {
            _clock = Substitute.For<IClock>();
            _dateCalculator = new DateCalculator(_clock);
        }

        [TestMethod]
        public void WillReturnsExtractTimeAdjustedToGmt()
        {
            _clock.UtcNow().Returns(new DateTime(2015, 10, 5, 13, 30, 0));

            var dates = _dateCalculator.Calculate();
            Assert.AreEqual(new DateTime(2015, 10, 5, 14, 30, 0), dates.ExtractDateTime);
        }

        [TestMethod]
        public void WillReturnsRequestDateAsTodayIfBeforeElevenPmLocal()
        {
            _clock.UtcNow().Returns(new DateTime(2015, 10, 5, 13, 30, 0));

            var dates = _dateCalculator.Calculate();
            Assert.AreEqual(new DateTime(2015, 10, 5), dates.RequestDate);
        }

        [TestMethod]
        public void WillReturnsRequestDateAsTomorrowIfOnElevenPmLocal()
        {
            _clock.UtcNow().Returns(new DateTime(2015, 10, 5, 22, 00, 0));

            var dates = _dateCalculator.Calculate();
            Assert.AreEqual(new DateTime(2015, 10, 6), dates.RequestDate);
        }

        [TestMethod]
        public void WillReturnsRequestDateAsTomorrowIfAfterElevenPmLocal()
        {
            _clock.UtcNow().Returns(new DateTime(2015, 10, 5, 22, 02, 0));

            var dates = _dateCalculator.Calculate();
            Assert.AreEqual(new DateTime(2015, 10, 6), dates.RequestDate);
        }
    }
}