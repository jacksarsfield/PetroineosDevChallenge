using System;
using System.Collections.Generic;
using System.Linq;
using PowerIntradayReporting.Model;
using Services;

namespace PowerIntradayReporting
{
    public class PositionAggregator : IPositionAggregator
    {
        private const int NoOfInterals = 24;

        public PowerPosition Aggregate(DateTime positionDate, IEnumerable<PowerTrade> powerTrades)
        {
            var powerTradesList = powerTrades.ToList();

            var position = new PowerPosition(positionDate, NoOfInterals);
            if (!powerTradesList.Any())
            {
                return position;
            }

            // as we do not have control over the number of intervals on the power trade 
            // the decision has been made that it is safer to throw an exception than to carry on and calc a position based on data that isnt in the expected shape
            if (powerTradesList.Any(x => x.Periods.Length != NoOfInterals))
            {
                throw new Exception(string.Format("All trades expected to have {0} periods.", NoOfInterals));
            }
                       
            for (int i = 0; i < NoOfInterals; i++)
            {
                position.Periods[i].Volume = powerTradesList.Sum(x => x.Periods[i].Volume);
            }

            return position;
        }
    }
}