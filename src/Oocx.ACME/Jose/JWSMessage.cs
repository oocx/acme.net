using Newtonsoft.Json;

namespace Oocx.Jose
{
    public class JwsMessage
    {
        [JsonProperty("header")]
        public JwsHeader Header { get; set; }

        [JsonProperty("protected")]
        public string Protected { get; set; }

        [JsonProperty("payload")]
        public string Payload { get; set; }

        [JsonProperty("signature")]
        public string Signature { get; set; }
    }
}