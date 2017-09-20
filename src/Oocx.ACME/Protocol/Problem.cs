using Newtonsoft.Json;

namespace Oocx.Acme.Protocol
{
    public class Problem
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("detail")]
        public string Detail { get; set; }
    }
}