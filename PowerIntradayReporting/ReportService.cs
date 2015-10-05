using System;
using System.Reactive.Linq;
using Core;
using log4net;
using PowerIntradayReporting.Constant;

namespace PowerIntradayReporting
{
    public class ReportService : IReportService
    {
        private readonly ILog _log;
        private readonly IConfigurationProvider _configurationProvider;
        private readonly IScheduleProvider _scheduleProvider;
        private readonly IReportGenerator _reportGenerator;
        private IDisposable _subscription;
        private string _reportFolder;

        public ReportService(ILog log, IConfigurationProvider configurationProvider, IScheduleProvider scheduleProvider, IReportGenerator reportGenerator)
        {
            _log = log;
            _configurationProvider = configurationProvider;
            _scheduleProvider = scheduleProvider;
            _reportGenerator = reportGenerator;
        }

        public void Start()
        {
            var intervalInMinutes = GetIntervalInMinutes();
            _reportFolder = GetReportFolder();
            _log.InfoFormat("ReportService Started, Report folder: {0}, Report interval: {1} mins.", _reportFolder, intervalInMinutes);
            _subscription = Observable.Interval(TimeSpan.FromMinutes(intervalInMinutes), _scheduleProvider.Default)
                                      .StartWith(-1L)
                                      .Subscribe(OnNext, OnError);            
        }

        public void Stop()
        {
            if (_subscription != null) { _subscription.Dispose(); }
            _log.Info("ReportService Stopped");
        }

        private void OnNext(long ticks)
        {
            try
            {
                _reportGenerator.Generate(_reportFolder);
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("ReportService Exception: {0}.", ex);
            }
        }

        private void OnError(Exception ex)
        {
            _log.ErrorFormat("ReportService Exception: {0}.", ex);
        }

        private int GetIntervalInMinutes()
        {
            var intervalInMinutes = _configurationProvider.GetSetting<int>(ReportConstants.IntradayReportIntervalInMins);
            if (intervalInMinutes == 0)
            {
                intervalInMinutes = ReportConstants.DefaultIntradayReportIntervalInMins;
                _log.WarnFormat("{0} not setting in config, defaulting to {1}", ReportConstants.IntradayReportIntervalInMins, ReportConstants.DefaultIntradayReportIntervalInMins);
            }

            return intervalInMinutes;
        }

        private string GetReportFolder()
        {
            var reportFolder = _configurationProvider.GetSetting(ReportConstants.IntradayReportFolder);
            if (string.IsNullOrEmpty(reportFolder))
            {
                reportFolder = ReportConstants.DefaultIntradayReportFolder;
                _log.WarnFormat("{0} not setting in config, defaulting to {1}", ReportConstants.IntradayReportFolder, ReportConstants.DefaultIntradayReportFolder);
            }

            return reportFolder;
        }
    }
}