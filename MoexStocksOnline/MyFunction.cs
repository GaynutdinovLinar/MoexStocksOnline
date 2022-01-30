using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.NetworkInformation;

namespace MoexStocksOnline
{
    internal static class MyFunction
    {
        internal static void  CheckInternet()
        {
            var status = new Ping().Send(@"www.moex.com").Status;

            if (status != IPStatus.Success) throw new Exception("Отсутствует подключение к интернету или сервис недоступен");
        }

        internal static string SearchInJToken(JToken jtok, string id, int columnSearch, int columnShow)
        {
            string res = "";

            foreach (var j in jtok)
            {
                if (j[columnSearch].ToString() == id)
                {
                    res = j[columnShow].ToString();
                    break;
                }
            }

            return res;
        }

        internal static string GetResponse(string url)
        {
            string response = null;

            using (WebClient wc = new WebClient())
            {
                response = wc.DownloadString(url);
            }

            return response;
        }

        internal static int? ParseInt(string s)
        {
            int i;
            if (int.TryParse(s, out i)) return i;
            else return null;
        }

        internal static decimal? ParseDecimal(string s)
        {
            decimal i;
            if (decimal.TryParse(s, out i)) return i;
            else return null;
        }
    }
}
