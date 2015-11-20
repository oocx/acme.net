using System;
using Newtonsoft.Json;

namespace Oocx.ACME.Protocol
{
    public class Challenge
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("uri")]
        public Uri Uri { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("error")]
        public Error Error { get; set; }

        [JsonProperty("tls")]
        public bool Tls { get; set; }

        [JsonProperty("validationRecord")]
        public ValidationRecord[] ValidationRecord { get; set; }
    }
}