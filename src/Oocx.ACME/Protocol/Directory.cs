using System;
using Newtonsoft.Json;

namespace Oocx.ACME.Protocol
{
    public class Directory
    {
        [JsonProperty("new-reg")]
        public Uri NewRegistration { get; set; }

        [JsonProperty("recover-reg")]
        public Uri RecoverRegistration { get; set; }

        [JsonProperty("new-authz")]
        public Uri NewAuthorization { get; set; }

        [JsonProperty("new-cert")]
        public Uri NewCertificate { get; set; }

        [JsonProperty("revoke-cert")]
        public Uri RevokeCertificate { get; set; }
    }
}
