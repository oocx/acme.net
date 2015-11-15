using Newtonsoft.Json;

namespace Oocx.ACME.Protocol
{
    public class SimpleHttpChallenge
    {
        [JsonProperty("type")]
        public string Type => "simpleHttp";

        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("tls")]
        public bool Tls { get; set; }
    }
}