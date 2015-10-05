using PowerIntradayReporting.Model;

namespace PowerIntradayReporting
{
    public interface IReportContentWriter
    {
        string Write(PowerPosition position);
    }
}