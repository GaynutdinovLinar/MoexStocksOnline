using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoexStocksOnline
{
    public class Board
    {
        internal Board(string boardid, string engine, string market, string boardTitle, bool isTraded)
        {
            BoardId = boardid;
            Engine = engine;
            Market = market;
            BoardTitle = boardTitle;
            IsTraded = isTraded;
        }

        #region Properties

        public string BoardId { get; internal set; }

        public string Engine { get; internal set; }

        public string Market { get; internal set; }

        public  string BoardTitle { get; internal set; }

        public bool IsTraded { get; internal set; }

        #endregion

        public async Task GetStocksAsync(ICollection<Stock> stocks) => await Task.Run(() => GetStocks(stocks));

        private void GetStocks(ICollection<Stock> stocks)
        {
            if (IsTraded)
            {
                string url = $"https://iss.moex.com/iss/engines/{Engine}/markets/{Market}/boards/{BoardId}/securities.json?iss.meta=off&iss.only=securities,marketdata&securities.columns=SECID,SHORTNAME,SECNAME&marketdata.columns=LAST,CHANGE";

                MyFunction.CheckInternet();

                if (stocks.Count > 0) stocks.Clear();

                string response = MyFunction.GetResponse(url);

                var json = JObject.Parse(response);

                var Jsecurities = json["securities"]["data"];
                var Jmarketdata = json["marketdata"]["data"];

                int i = 0;
                foreach (var sec in Jsecurities)
                {
                    Stock st = new Stock(this, sec[0].ToString(), sec[1].ToString(), sec[2].ToString(), MyFunction.ParseDouble(Jmarketdata[i][0].ToString()), MyFunction.ParseDouble(Jmarketdata[i][1].ToString()));

                    stocks.Add(st);
                    i++;
                }
            }
        }
    }
}
