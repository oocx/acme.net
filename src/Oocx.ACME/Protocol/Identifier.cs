using Newtonsoft.Json;

namespace Oocx.ACME.Protocol
{
    public class Identifier
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}