using Newtonsoft.Json;

namespace Oocx.Acme.Protocol
{
    public class AuthorizationRequest
    {
        [JsonProperty("resource")]
        public string Resource { get; set; }

        [JsonProperty("identifier")]
        public Identifier Identifier { get; set; }
    }
}