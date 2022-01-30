using System;

namespace MoexStocksOnline
{
    public class DayCost
    {
        internal DayCost(DateTime day, decimal? cost)
        {
            Day = day;
            Cost = cost;
        }

        public DateTime Day{ get; internal set;}

        public decimal? Cost { get; internal set;}
    }
}
