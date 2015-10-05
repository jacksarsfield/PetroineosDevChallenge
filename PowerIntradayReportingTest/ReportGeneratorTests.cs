using System;
using Core;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PowerIntradayReporting;
using PowerIntradayReporting.Model;
using Services;

namespace PowerIntradayReportingTest
{
    [TestClass]
    public class ReportGeneratorTests
    {
        private ILog _log;
        private IClock _clock;
        private IPowerService _powerService;
        private IPositionAggregator _positionAggregator;
        private IFileNameGenerator _fileNameGenerator;
        private IReportContentWriter _reportContentWriter;
        private IFile _file;
        private IReportGenerator _reportGenerator;

        private string _reportFolder;
        private DateTime _utcNow;
        private DateTime _extractLocalTime;
        private PowerTrade _powerTradeOne;
        private PowerTrade _powerTradeTwo;
        private PowerTrade[] _powerTrades;
        private PowerPosition _powerPosition;
        private string _fileName;
        private string _content;

        [TestInitialize]
        public void Init()
        {
            _log = Substitute.For<ILog>();
            _clock = Substitute.For<IClock>();
            _powerService = Substitute.For<IPowerService>();
            _positionAggregator = Substitute.For<IPositionAggregator>();
            _fileNameGenerator = Substitute.For<IFileNameGenerator>();
            _reportContentWriter = Substitute.For<IReportContentWriter>();
            _file = Substitute.For<IFile>();
            _reportGenerator = new ReportGenerator(_log, _clock, _powerService, _positionAggregator, _fileNameGenerator, _reportContentWriter, _file);

            _reportFolder = @"C:\Temp\";
            _utcNow = new DateTime(2015, 10, 12, 13, 30, 0);
            _extractLocalTime = new DateTime(2015, 10, 12, 14, 30, 0);

            _powerTradeOne = new PowerTrade();
            _powerTradeTwo = new PowerTrade();
            _powerTrades = new[] { _powerTradeOne, _powerTradeTwo };
            _powerPosition = new PowerPosition();
            _fileName = "PowerPositions.csv";
            _content = "Local time, Volume etc";

            _clock.UtcNow().Returns(_utcNow);
            _powerService.GetTrades(_extractLocalTime).Returns(_powerTrades);
            _positionAggregator.Aggregate(_extractLocalTime, _powerTrades).Returns(_powerPosition);
            _fileNameGenerator.Generate(_extractLocalTime).Returns(_fileName);
            _reportContentWriter.Write(_powerPosition).Returns(_content);
        }

        [TestMethod]
        public void WillCallAllComponentPassingDataAsExpected()
        {
            _reportGenerator.Generate(_reportFolder);

            _log.Received(1).InfoFormat("ReportGenerator started with extract time: {0}", _extractLocalTime);
            _powerService.Received(1).GetTrades(_extractLocalTime);
            _log.Received(1).InfoFormat("{0} trade returned", _powerTrades.Length);
            _positionAggregator.Received(1).Aggregate(_extractLocalTime, _powerTrades);
            _fileNameGenerator.Received(1).Generate(_extractLocalTime);
            _reportContentWriter.Received(1).Write(_powerPosition);
            _file.Received(1).WriteAllText(_reportFolder + _fileName, _content);
            _log.Received(1).InfoFormat("ReportGenerator complete: {0}", _reportFolder + _fileName);
        }

        [TestMethod]
        public void WillRetryAndContinueLoggingAWarningIfPowerServiceThrowsException()
        {
            _powerService.GetTrades(_extractLocalTime).Returns(x => { throw new Exception("Error on 1st call"); }, x => _powerTrades);

            _reportGenerator.Generate(_reportFolder);

            _log.Received(1).Warn("Retrying after error during GetTrades");
            _file.Received(1).WriteAllText(_reportFolder + _fileName, _content);
        }
    }
}