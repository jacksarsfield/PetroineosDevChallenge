using log4net;
using StructureMap.Configuration.DSL;

namespace Core.DependencyInjection
{
    public class CoreRegistry : Registry
    {
        public CoreRegistry()
        {
            For<IClock>().Use<Clock>();
            For<IConfigurationProvider>().Use<ConfigurationProvider>();
            For<IFile>().Use<File>();
            For<ILog>().Use(LogManager.GetLogger(GetType()));
            For<IScheduleProvider>().Use<ScheduleProvider>();            
        }
    }
}