using System;
using System.Collections.Generic;
using PowerIntradayReporting.Model;
using Services;

namespace PowerIntradayReporting
{
    public interface IPositionAggregator
    {
        PowerPosition Aggregate(DateTime date, IEnumerable<PowerTrade> powerTrades);
    }
}