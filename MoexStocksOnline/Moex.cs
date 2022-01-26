using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoexStocksOnline
{
    public static class Moex
    {
        public static async Task GetBoardsAsync(ICollection<Board> coll) => await Task.Run(()=> GetBoards(coll));

        public static void GetBoards(ICollection<Board> coll)
        {
            string url = "http://iss.moex.com/iss/index.json?iss.meta=off&iss.only=engines,markets,boards&engines.columns=id,name&markets.columns=id,market_name&boards.columns=engine_id,market_id,boardid,board_title,is_traded";

            MyFunction.CheckInternet();

            string response = MyFunction.GetResponse(url);

            var json = JObject.Parse(response);

            var Jengines = json["engines"]["data"];
            var Jmarkets = json["markets"]["data"];
            var Jboards = json["boards"]["data"];

            if (coll.Count > 0) coll.Clear();

            foreach (var sec in Jboards)
            {
                bool isTraded;
                if (sec[4].ToString() == "0") isTraded = false;
                else isTraded = true;

                Board b = new Board(sec[2].ToString(), MyFunction.SearchInJToken(Jengines, sec[0].ToString(), 0, 1), MyFunction.SearchInJToken(Jmarkets, sec[1].ToString(), 0, 1), sec[3].ToString(), isTraded);

                coll.Add(b);
            }
        }

        public static Stock SearchStock(this ICollection<Stock> ls, string secid)
        {
            if (ls != null)
            {
                Stock st = null;

                foreach (var l in ls)
                {
                    if (l.Secid == secid)
                    {
                        st = l;
                        break;
                    }
                }

                return st;
            }
            else return null;
        }

        public static Board SearchBoard(this ICollection<Board> ls, string boardid)
        {
            if (ls != null)
            {
                Board st = null;

                foreach (var l in ls)
                {

                    if (l.BoardId == boardid)
                    {
                        st = l;
                        break;
                    }
                }

                return st;
            }
            else return null;
        }
    }
}
