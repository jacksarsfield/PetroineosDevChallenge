using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PowerIntradayReporting;
using Services;

namespace PowerIntradayReportingTest
{
    [TestClass]
    public class PositionAggregatorTests
    {
        private IPositionAggregator _aggregator;
        private DateTime _date;
        private List<PowerTrade> _trades;

        [TestInitialize]
        public void Init()
        {
            _aggregator = new PositionAggregator();
            _date = new DateTime(2015, 7, 8);
            _trades = new List<PowerTrade>();
        }

        [TestMethod]
        public void WillReturnZeroPositionIfNoTrades()
        {
            var position = _aggregator.Aggregate(_date, new List<PowerTrade>());

            Assert.IsNotNull(position);
            Assert.AreEqual(24, position.Periods.Length);
            for (int i = 0; i < position.Periods.Length; i++)
            {
                Assert.AreEqual(0, position.Periods[i].Volume);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void WillThrowAnExceptionIfAnyTradesDoNotHaveTwentyFourPeriods()
        {
            var tradeOne = PowerTrade.Create(new DateTime(2015, 1, 3), 24);
            var tradeTwo = PowerTrade.Create(new DateTime(2015, 1, 3), 20);
            _trades.Add(tradeOne);
            _trades.Add(tradeTwo);
            _aggregator.Aggregate(_date, _trades);
        }

        [TestMethod]
        public void WillSumAllTradePeriodsAndSetOnPosition()
        {
            var tradeOne = PowerTrade.Create(new DateTime(2015, 1, 3), 24);
            for (int i = 0; i < 24; i++)
            {
                tradeOne.Periods[i].Volume = 10;
            }

            var tradeTwo = PowerTrade.Create(new DateTime(2015, 1, 3), 24);
            for (int i = 0; i < 24; i++)
            {
                tradeTwo.Periods[i].Volume = -5;
            }

            _trades.Add(tradeOne);
            _trades.Add(tradeTwo);

            var position = _aggregator.Aggregate(_date, _trades);

            Assert.IsNotNull(position);
            Assert.AreEqual(24, position.Periods.Length);
            for (int i = 0; i < position.Periods.Length; i++)
            {
                Assert.AreEqual(5, position.Periods[i].Volume);
            }
        }
    }
}