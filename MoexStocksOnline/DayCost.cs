using System;

namespace MoexStocksOnline
{
    public class DayCost
    {
        internal DayCost(DateTime day, double? cost)
        {
            Day = day;
            Cost = cost;
        }

        public DateTime Day{ get; internal set;}

        public double? Cost { get; internal set;}
    }
}
