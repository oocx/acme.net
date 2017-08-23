using Newtonsoft.Json;

namespace Oocx.ACME.Jose
{
    public class JWSMessage
    {
        [JsonProperty("header")]
        public JWSHeader Header { get; set; }

        [JsonProperty("protected")]
        public string Protected { get; set; }

        [JsonProperty("payload")]
        public string Payload { get; set; }

        [JsonProperty("signature")]
        public string Signature { get; set; }
    }
}