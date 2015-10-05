using System;
using System.ServiceProcess;
using System.Threading.Tasks;
using log4net;
using PowerIntradayReporting;
using PowerReportService.DependencyInjection;
using StructureMap;

namespace PowerReportService
{
    partial class Service : ServiceBase
    {
        private readonly ILog _log;
        private readonly IReportService _intradayReportService;

        public Service()
        {
            InitializeComponent();

            _log = LogManager.GetLogger(typeof(Service));
            var container = new Container(new PowerReportServiceRegistry());
            _intradayReportService = container.GetInstance<IReportService>();
        }

        public void Start(string[] args)
        {
            OnStart(args);
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                var task = Task.Factory.StartNew(_intradayReportService.Start);
                task.Wait(10000);
            }
            catch (Exception ex)
            {
                _log.Error("Service failed to start", ex);
                throw;
            }
        }

        protected override void OnStop()
        {
            try
            {
                _intradayReportService.Stop();
            }
            catch (Exception ex)
            {
                _log.Error("Service failed to stop", ex);
                throw;
            }
        }
    }
}
