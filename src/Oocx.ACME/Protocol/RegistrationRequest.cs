using Newtonsoft.Json;

namespace Oocx.ACME.Protocol
{
    public abstract class RegistrationRequest
    {
        [JsonProperty("jwk")]
        public JsonWebKey Key { get; set; }
        
        [JsonProperty("contact")]
        public string[] Contact { get; set; }

        [JsonProperty("agreement")]
        public string Agreement { get; set; }

        [JsonProperty("authorizations")]
        public string Authorization { get; set; }

        [JsonProperty("certificates")]
        public string Certificates { get; set; }
    }

    public class NewRegistrationRequest : RegistrationRequest
    {
        [JsonProperty("resource")]
        public string Resource { get; } = "new-reg";
    }

    public class UpdateRegistrationRequest : RegistrationRequest
    {
        [JsonProperty("resource")]
        public string Resource { get; } = "reg";
    }
}