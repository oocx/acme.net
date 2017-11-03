using System;

using Newtonsoft.Json;

namespace Oocx.Acme.Protocol
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
        public Challenge[] Challenges { get; set; }

        [JsonProperty("combinations")]
        public int[][] Combinations { get; set; }

        [JsonIgnore]
        public Uri Location { get; set; }
    }
}

// https://tools.ietf.org/html/draft-ietf-acme-acme-01#section-5.3