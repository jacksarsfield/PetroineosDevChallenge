using System.ServiceProcess;

namespace PowerReportService
{
    using System;
    using log4net.Config;

    static class Program
    {
        static void Main(string[] args)
        {
            XmlConfigurator.Configure();

            if (Environment.UserInteractive)
            {
                var service = new Service();
                Console.WriteLine("Service has started, press any key to stop");
                service.Start(args);
                Console.ReadKey();
                service.Stop();
            }
            else
            {
                ServiceBase.Run(new Service());
            }
        }
    }
}
