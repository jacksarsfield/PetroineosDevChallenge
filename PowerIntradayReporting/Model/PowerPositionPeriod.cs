using System;

namespace PowerIntradayReporting.Model
{
    public class PowerPositionPeriod
    {
        public DateTime PeriodDateTime { get; internal set; }
        public double Volume { get; set; }
    }
}