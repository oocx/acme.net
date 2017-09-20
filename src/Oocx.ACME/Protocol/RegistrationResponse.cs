using System;
using Newtonsoft.Json;

using Oocx.Jose;

namespace Oocx.Acme.Protocol
{
    public class RegistrationResponse
    {
        [JsonProperty("key")]
        public Jwk Key { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("initialIp")]
        public string InitialIp { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("contact")]
        public string[] Contact { get; set; }

        [JsonProperty("agreement")]
        public string Agreement { get; set; }

        [JsonProperty("authorizations")]
        public string Authorization { get; set; }

        [JsonProperty("certificates")]
        public string Certificates { get; set; }

        public string Location { get; set; }
    }
}