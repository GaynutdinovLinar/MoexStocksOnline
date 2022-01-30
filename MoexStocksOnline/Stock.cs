using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoexStocksOnline
{
    public class Stock
    {
        internal Stock (Board board, string secid, string shortname, string secname, double? changeCost, double? lastCost)
        {
            Secid = secid;
            Shortname = shortname;
            Secname = secname;
            ChangeCost = changeCost;
            LastCost = lastCost;

            if (changeCost != null && lastCost != null) StartCost = lastCost - changeCost;
            else ChangeCost = null;

            BoardStock = board;
        }

        #region Properties

        public string Secid { get; internal set; }
        public string Shortname { get; internal set; }
        public string Secname { get; internal set; }
        public double? StartCost { get; internal set; }
        public double? LastCost { get; internal set; }
        public double? ChangeCost { get; internal set; }
        public Board BoardStock { get; internal set; }

        #endregion

        public async Task GetCostPeriodStockAsync(ICollection<DayCost> daycost, DateTime Day)
        {
            await GetCostPeriodStockAsync(daycost, Day, Day);
        }

        public async Task GetCostPeriodStockAsync(ICollection<DayCost> daycost, DateTime startDate, DateTime endDate) => await Task.Run(()=> GetCostPeriodStock(daycost, startDate, endDate));

        private void GetCostPeriodStock(ICollection<DayCost> daycost, DateTime startDate, DateTime endDate)
        {
            if (startDate <= endDate)
            {
                int razn = (endDate - startDate).Days;

                for (int start = 0; start <= razn; start += 100)
                {
                    string st = startDate.AddDays(start).ToString("yyyy-MM-dd");
                    string en;

                    if (razn - start < 100) en = startDate.AddDays(start + (razn - start)).ToString("yyyy-MM-dd");
                    else en = startDate.AddDays(start + 100).ToString("yyyy-MM-dd");

                    string url = $"https://iss.moex.com/iss/history/engines/{BoardStock.Engine}/markets/{BoardStock.Market}/boards/{BoardStock.BoardId}/securities/{Secid}.json?iss.meta=off&iss.only=history&from={st}&till={en}&history.columns=TRADEDATE,CLOSE";

                    MyFunction.CheckInternet();

                    string response = MyFunction.GetResponse(url);

                    var json = JObject.Parse(response);

                    var Jhistory = json["history"]["data"];

                    foreach (var sec in Jhistory)
                    {
                        DayCost dc = new DayCost((DateTime)sec[0], MyFunction.ParseDouble(sec[1].ToString()));

                        daycost.Add(dc);
                    }
                }
            }
        }

    }
}