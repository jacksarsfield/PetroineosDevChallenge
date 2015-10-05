using System;
using System.Collections.Generic;
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
        private IDateCalculator _dateCalculator;
        private IPowerService _powerService;
        private IPositionAggregator _positionAggregator;
        private IFileNameGenerator _fileNameGenerator;
        private IReportContentWriter _reportContentWriter;
        private IFile _file;
        private IReportGenerator _reportGenerator;

        private string _reportFolder;
        private DateResult _dates;
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
            _dateCalculator = Substitute.For<IDateCalculator>();
            _powerService = Substitute.For<IPowerService>();
            _positionAggregator = Substitute.For<IPositionAggregator>();
            _fileNameGenerator = Substitute.For<IFileNameGenerator>();
            _reportContentWriter = Substitute.For<IReportContentWriter>();
            _file = Substitute.For<IFile>();
            _reportGenerator = new ReportGenerator(_log, _dateCalculator, _powerService, _positionAggregator, _fileNameGenerator, _reportContentWriter, _file);

            _reportFolder = @"C:\Temp\";
            _dates = new DateResult { ExtractDateTime = new DateTime(2015, 10, 5, 23, 34, 0), RequestDate = new DateTime(2015, 10, 6) };

            _powerTradeOne = new PowerTrade();
            _powerTradeTwo = new PowerTrade();
            _powerTrades = new[] { _powerTradeOne, _powerTradeTwo };
            _powerPosition = new PowerPosition();
            _fileName = "PowerPositions.csv";
            _content = "Local time, Volume etc";

            _dateCalculator.Calculate().Returns(_dates);
            _powerService.GetTrades(_dates.RequestDate).Returns(_powerTrades);
            _positionAggregator.Aggregate(_dates.RequestDate, Arg.Is<List<PowerTrade>>(x => x[0] == _powerTradeOne && x[1] == _powerTradeTwo)).Returns(_powerPosition);
            _fileNameGenerator.Generate(_dates.ExtractDateTime).Returns(_fileName);
            _reportContentWriter.Write(_powerPosition).Returns(_content);
        }

        [TestMethod]
        public void WillCallAllComponentPassingDataAsExpected()
        {
            _reportGenerator.Generate(_reportFolder);

            _log.Received(1).Info("ReportGenerator started");
            _dateCalculator.Received(1).Calculate();
            _log.Received().InfoFormat("Report ExtractDateTime: {0}, PowerService request date: {1}", _dates.ExtractDateTime, _dates.RequestDate);
            _powerService.Received(1).GetTrades(_dates.RequestDate);
            _log.Received(1).InfoFormat("{0} trade returned", _powerTrades.Length);
            _positionAggregator.Received(1).Aggregate(_dates.RequestDate, Arg.Is<List<PowerTrade>>(x => x.Count == 2 && x[0] == _powerTradeOne && x[1] == _powerTradeTwo));
            _fileNameGenerator.Received(1).Generate(_dates.ExtractDateTime);
            _reportContentWriter.Received(1).Write(_powerPosition);
            _file.Received(1).WriteAllText(_reportFolder + _fileName, _content);
            _log.Received(1).InfoFormat("ReportGenerator complete: {0}", _reportFolder + _fileName);
        }

        [TestMethod]
        public void WillRetryAndContinueLoggingAWarningIfPowerServiceThrowsException()
        {
            _powerService.GetTrades(_dates.RequestDate).Returns(x => { throw new Exception("Error on 1st call"); }, x => _powerTrades);

            _reportGenerator.Generate(_reportFolder);

            _log.Received(1).Warn("Retrying after error during GetTrades");
            _file.Received(1).WriteAllText(_reportFolder + _fileName, _content);
        }
    }
}