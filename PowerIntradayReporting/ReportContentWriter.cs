using System.Text;
using PowerIntradayReporting.Model;

namespace PowerIntradayReporting
{
    public class ReportContentWriter : IReportContentWriter
    {
        public string Write(PowerPosition position)
        {
            StringBuilder content = new StringBuilder("Local Time, Volume");
            foreach (var period in position.Periods)
            {
                content.AppendLine();
                content.AppendFormat("{0:HH:mm}, {1}", period.PeriodDateTime, period.Volume);
            }
            
            return content.ToString();
        }
    }
}