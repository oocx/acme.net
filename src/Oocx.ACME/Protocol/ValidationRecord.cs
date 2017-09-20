using System;
using Newtonsoft.Json;

namespace Oocx.Acme.Protocol
{
    public class ValidationRecord
    {
        [JsonProperty("addressResolved")]
        public string AddressResolved { get; set; }

        [JsonProperty("addressUsed")]
        public string AddressUsed { get; set; }

        [JsonProperty("hostname")]
        public string HostName { get; set; }

        [JsonProperty("port")]
        public int Port { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }
    }
}