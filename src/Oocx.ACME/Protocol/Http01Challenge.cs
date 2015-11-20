using Newtonsoft.Json;

namespace Oocx.ACME.Protocol
{
    public class Http01Challenge
    {
        [JsonProperty("type")]
        public string Type => "http-01";

        [JsonProperty("token")]
        public string Token { get; set; }
    }
}