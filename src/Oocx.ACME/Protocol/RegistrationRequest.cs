using System;

using Newtonsoft.Json;

using Oocx.Jose;

namespace Oocx.Acme.Protocol
{
    public abstract class RegistrationRequest
    {
        [JsonProperty("jwk")]
        public Jwk Key { get; set; }

        [JsonProperty("contact")]
        public string[] Contact { get; set; }

        [JsonProperty("agreement")]
        public string Agreement { get; set; }

        [JsonProperty("authorizations")]
        public string Authorizations { get; set; }

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
        public UpdateRegistrationRequest() { }

        public UpdateRegistrationRequest(string location, string agreement, string[] contact)
        {
            Location  = location ?? throw new ArgumentNullException(nameof(location));
            Agreement = agreement;
            Contact   = contact;
        }

        [JsonIgnore]
        public string Location { get; }

        [JsonProperty("resource")]
        public string Resource { get; } = "reg";
    }
}