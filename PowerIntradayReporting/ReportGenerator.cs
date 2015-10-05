using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core;
using log4net;
using PowerIntradayReporting.Constant;
using Services;

namespace PowerIntradayReporting
{
    public class ReportGenerator : IReportGenerator
    {
        private readonly ILog _log;
        private readonly IClock _clock;
        private readonly IPowerService _powerService;
        private readonly IPositionAggregator _positionAggregator;
        private readonly IFileNameGenerator _fileNameGenerator;
        private readonly IReportContentWriter _reportContentWriter;
        private readonly IFile _file;

        public ReportGenerator(ILog log, IClock clock, IPowerService powerService, IPositionAggregator positionAggregator, IFileNameGenerator fileNameGenerator, IReportContentWriter reportContentWriter, IFile file)
        {
            _log = log;
            _clock = clock;
            _powerService = powerService;
            _positionAggregator = positionAggregator;
            _fileNameGenerator = fileNameGenerator;
            _reportContentWriter = reportContentWriter;
            _file = file;
        }

        public void Generate(string reportFolder)
        {
            var extractLocalTime = TimeZoneInfo.ConvertTimeFromUtc(_clock.UtcNow(), TimeZoneInfo.FindSystemTimeZoneById(ReportConstants.GmtStandardTime));
            _log.InfoFormat("ReportGenerator started with extract time: {0}", extractLocalTime);

            // added a retry to the powerservice as this is an external call and not something we have control over
            // retry could be changed to catch specific exceptions
            var trades = new RetryBlock<IEnumerable<PowerTrade>>(() => _powerService.GetTrades(extractLocalTime))
            .WithMaxRetries(3)
            .WithWaitBetweenRetries(1000)
            .WithActionBetweenRetries(() => _log.Warn(("Retrying after error during GetTrades")))
            .Execute();

            _log.InfoFormat("{0} trade returned", trades.Count());

            var position = _positionAggregator.Aggregate(extractLocalTime, trades);
        
            var fileName = _fileNameGenerator.Generate(extractLocalTime);
            var content = _reportContentWriter.Write(position);
            var fullFilePath = Path.Combine(reportFolder, fileName);
            _file.WriteAllText(fullFilePath, content);
            _log.InfoFormat("ReportGenerator complete: {0}", fullFilePath);
        }
    }
}