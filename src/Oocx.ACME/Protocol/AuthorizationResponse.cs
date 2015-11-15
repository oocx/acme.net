using System;
using Newtonsoft.Json;

namespace Oocx.ACME.Protocol
{
    public class AuthorizationResponse
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("expires")]
        public string Expires { get; set; }

        [JsonProperty("identifier")]
        public Identifier Identifier { get; set; }

        [JsonProperty("challenges")]
        public Challange[] Challenges { get; set; }

        [JsonProperty("combinations")]
        public int[][] Combinations { get; set; }

        [JsonIgnore]
        public Uri Location { get; set; }
    }
}