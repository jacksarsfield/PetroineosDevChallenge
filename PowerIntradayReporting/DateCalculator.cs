using System;
using System.Runtime.Remoting.Messaging;
using Core;
using PowerIntradayReporting.Constant;
using PowerIntradayReporting.Model;

namespace PowerIntradayReporting
{
    public class DateCalculator : IDateCalculator
    {
        private readonly IClock _clock;

        public DateCalculator(IClock clock)
        {
            _clock = clock;
        }

        public DateResult Calculate()
        {
            var extractDateTime = TimeZoneInfo.ConvertTimeFromUtc(_clock.UtcNow(), TimeZoneInfo.FindSystemTimeZoneById(ReportConstants.GmtStandardTime));

            var requestDate = extractDateTime.Date;
            if (extractDateTime.TimeOfDay >= TimeSpan.FromHours(23))
            {
                requestDate = extractDateTime.Date.AddDays(1);
            }

            return new DateResult { ExtractDateTime = extractDateTime, RequestDate = requestDate };
        }
    }
}