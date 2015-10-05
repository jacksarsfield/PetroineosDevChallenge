using System;
using Core;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PowerIntradayReporting;
using PowerIntradayReporting.DependencyInjection;
using StructureMap;

namespace PowerIntradayReportingTest
{
    [TestClass]
    public class ReportServiceTests
    {
        private ILog _log;
        private IConfigurationProvider _configurationProvider;
        private TestSchedulers _scheduleProvider;
        private IReportGenerator _generator;
        private IReportService _service;

        private string _reportFolder;
         
        [TestInitialize]
        public void Init()
        {
            _log = Substitute.For<ILog>();
            _configurationProvider = Substitute.For<IConfigurationProvider>();
            _scheduleProvider = new TestSchedulers();
            _generator = Substitute.For<IReportGenerator>();
            
            _reportFolder = @"C:\Temp\";
            _configurationProvider.GetSetting("IntradayReportFolder").Returns(_reportFolder);

            _service = new ReportService(_log, _configurationProvider, _scheduleProvider, _generator);
        }

        [TestMethod]
        public void WillBeAbleToCreateInstanceIfEverythingIsRegistered()
        {
            using (var container = new Container(new IntradayReportRegistry()))
            {
                var service = container.GetInstance<IReportService>();
                Assert.IsNotNull(service);
            }
        }
        
        [TestMethod]
        public void WillLogStartOfServiceWithConfigSettings()
        {
            _service.Start();
            _log.Received(1).InfoFormat("ReportService Started, Report folder: {0}, Report interval: {1} mins.", @"C:\Temp\", 5);
        }

        [TestMethod]
        public void WillLogStopOfService()
        {
            _service.Stop();
            _log.Received(1).Info("ReportService Stopped");
        }

        [TestMethod]
        public void WillLogErrorIfGeneratorHasExcption()
        {
            var ex = new Exception("Error in generate");
            _generator.When(x => x.Generate(_reportFolder)).Do(x => { throw ex; });

            _service.Start();
            _log.Received(1).ErrorFormat("ReportService Exception: {0}.", ex);
        }

        [TestMethod]
        public void WillUseDefaultReportFolderIfNotSetInConfigLoggingWarning()
        {
            _configurationProvider.GetSetting("IntradayReportFolder").Returns(x => { return null; });

            _service.Start();

            _log.Received(1).WarnFormat("{0} not setting in config, defaulting to {1}", "IntradayReportFolder", @"C:\Temp\");
            _generator.Received(1).Generate(_reportFolder);
        }

        [TestMethod]
        public void WillCallGeneratorImmediatelyThenEveryOneMinuteIfSetInConfigUsingConfiguredReportFolder()
        {
            _configurationProvider.GetSetting<int>("IntradayReportIntervalInMins").Returns(1);

            _service.Start();

            _generator.Received(1).Generate(_reportFolder);
            _scheduleProvider.Default.AdvanceBy(TimeSpan.FromMinutes(1).Ticks - 1);
            _generator.Received(1).Generate(_reportFolder);
            _scheduleProvider.Default.AdvanceBy(2);
            _generator.Received(2).Generate(_reportFolder);
        }

        [TestMethod]
        public void WillCallGeneratorImmediatelyThenEvery10MinuteIfSetInConfigUsingConfiguredReportFolder()
        {
            _configurationProvider.GetSetting<int>("IntradayReportIntervalInMins").Returns(10);

            _service.Start();

            _generator.Received(1).Generate(_reportFolder);
            _scheduleProvider.Default.AdvanceBy(TimeSpan.FromMinutes(10).Ticks - 1);
            _generator.Received(1).Generate(_reportFolder);
            _scheduleProvider.Default.AdvanceBy(2);
            _generator.Received(2).Generate(_reportFolder);
        }

        [TestMethod]
        public void WillCallGeneratorImmediatelyThenEvery5MinuteIfConfigHasNotBeenSetLoggingWarning()
        {
            _service.Start();

            _log.Received(1).WarnFormat("{0} not setting in config, defaulting to {1}", "IntradayReportIntervalInMins", 5);

            _generator.Received(1).Generate(_reportFolder);
            _scheduleProvider.Default.AdvanceBy(TimeSpan.FromMinutes(5).Ticks - 1);
            _generator.Received(1).Generate(_reportFolder);
            _scheduleProvider.Default.AdvanceBy(2);
            _generator.Received(2).Generate(_reportFolder);
        }
    }
}
