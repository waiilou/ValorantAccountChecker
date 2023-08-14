using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace valorant.Models
{
    public class MMR : BaseResponse
    {
        public class Images
        {
            public string Small { get; set; }
            public string Large { get; set; }
            public string TriangleDown { get; set; }
            public string TriangleUp { get; set; }
        }

        public class Data
        {
            public int CurrentTier { get; set; }
            public string CurrentTierPatched { get; set; }
            public Images Images { get; set; }
            [JsonProperty("ranking_in_tier")]
            public int RankingInTier { get; set; }
            [JsonProperty("mmr_change_to_last_game")]
            public int MmrChangeToLastGame { get; set; }
            public int Elo { get; set; }
            public string Name { get; set; }
            public string Tag { get; set; }
            public bool Old { get; set; }
        }

        public class JsonModel
        {
            public int Status { get; set; }
            public Data Data { get; set; }
        }
    }
}
