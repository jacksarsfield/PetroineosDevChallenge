using Core.DependencyInjection;
using StructureMap.Configuration.DSL;
using PowerIntradayReporting.DependencyInjection;

namespace PowerReportService.DependencyInjection
{
    public class PowerReportServiceRegistry : Registry
    {
        public PowerReportServiceRegistry()
        {
            IncludeRegistry<CoreRegistry>();
            IncludeRegistry<IntradayReportRegistry>();
        } 
    }
}