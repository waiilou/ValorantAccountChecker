using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static valorant.Models.Account;

namespace valorant.Models
{
    public class MMRHistory : BaseResponse
    {

        public class Images
        {
            public string Small { get; set; }
            public string Large { get; set; }
            public string TriangleDown { get; set; }
            public string TriangleUp { get; set; }
        }

        public class Map
        {
            public string Name { get; set; }
            public string Id { get; set; }
        }

        public class Datum
        {
            public int CurrentTier { get; set; }
            public string CurrentTierPatched { get; set; }
            public Images Images { get; set; }
            public string MatchId { get; set; }
            public Map Map { get; set; }
            public string SeasonId { get; set; }
            [JsonProperty("ranking_in_tier")]
            public int RankingInTier { get; set; }
            [JsonProperty("mmr_change_to_last_game")]
            public int MmrChangeToLastGame { get; set; }
            public int Elo { get; set; }
            public string Date { get; set; }
            public int DateRaw { get; set; }
        }

        public class JsonModel
        {
            public int Status { get; set; }
            public string Name { get; set; }
            public string Tag { get; set; }
            public List<Datum> Data { get; set; }
        }
    }
}
