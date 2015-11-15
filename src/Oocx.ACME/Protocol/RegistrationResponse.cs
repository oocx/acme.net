using Newtonsoft.Json;

namespace Oocx.ACME.Protocol
{
    public class RegistrationResponse
    {
        [JsonProperty("key")]
        public JsonWebKey Key { get; set; }

        [JsonProperty("resource")]
        public string Resource { get; set; }

        [JsonProperty("contact")]
        public string[] Contact { get; set; }

        [JsonProperty("agreement")]
        public string Agreement { get; set; }

        [JsonProperty("authorizations")]
        public string Authorization { get; set; }

        [JsonProperty("certificates")]
        public string Certificates { get; set; }
    }
}