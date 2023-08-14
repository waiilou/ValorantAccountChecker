using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace valorant.Models
{

        public class Account : BaseResponse
        {
            public class PlayerCard
            {
                public string Small { get; set; }
                public string Large { get; set; }
                public string Wide { get; set; }
                public string Id { get; set; }
            }

            public class PlayerData
            {
                public string Puuid { get; set; }
                public string Region { get; set; }
                [JsonProperty("account_level")]
                public int AccountLevel { get; set; }
                public string Name { get; set; }
                public string Tag { get; set; }
                public PlayerCard Card { get; set; }
                [JsonProperty("last_update")]
                public string LastUpdate { get; set; }
                [JsonProperty("last_update_raw")]
                public int LastUpdateRaw { get; set; }
            }

            public class JsonResponse
            {
                public int Status { get; set; }
                public PlayerData Data { get; set; }
            }

        }
    }