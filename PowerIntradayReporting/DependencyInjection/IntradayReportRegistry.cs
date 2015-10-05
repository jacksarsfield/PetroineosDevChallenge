using Core.DependencyInjection;
using Services;
using StructureMap.Configuration.DSL;

namespace PowerIntradayReporting.DependencyInjection
{
    public class IntradayReportRegistry : Registry
    {
        public IntradayReportRegistry()
        {
            IncludeRegistry<CoreRegistry>();

            For<IDateCalculator>().Use<DateCalculator>();
            For<IFileNameGenerator>().Use<FileNameGenerator>();
            For<IReportContentWriter>().Use<ReportContentWriter>();
            For<IReportGenerator>().Use<ReportGenerator>();
            For<IReportService>().Use<ReportService>();
            For<IPositionAggregator>().Use<PositionAggregator>();
            For<IPowerService>().Use<PowerService>();
        } 
    }
}