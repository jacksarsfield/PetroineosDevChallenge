using System;
using System.Collections.Generic;

namespace PowerIntradayReporting.Model
{
    public class PowerPosition
    {
        public PowerPosition()
        { }

        public PowerPosition(DateTime date, int numberOfPeriods)
        {
            Date = date;
            var start = date.AddHours(-1);
            var periods = new List<PowerPositionPeriod>();
            for (int i = 0; i < numberOfPeriods; i++)
            {
                periods.Add(new PowerPositionPeriod { PeriodDateTime = start, Volume = 0 });
                start = start.AddHours(1);
            }

            Periods = periods.ToArray();
        }

        public DateTime Date { get; private set; }
        public PowerPositionPeriod[] Periods { get; private set; }
    }
}