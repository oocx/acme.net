using Newtonsoft.Json;

namespace Oocx.ACME.Protocol
{
    public class AuthorizationRequest
    {
        [JsonProperty("resource")]
        public string Resource { get; set; }

        [JsonProperty("identifier")]
        public Identifier Identifier { get; set; }
    }
}