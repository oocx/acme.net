using System;
using Newtonsoft.Json;

namespace Oocx.Acme.Protocol
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

        [JsonProperty("key-change")]
        public Uri KeyChange { get; set; }

        [JsonProperty("meta")]
        public DirectoryMeta Meta { get; set; }
    }

    public class DirectoryMeta
    {
        [JsonProperty("terms-of-service")]
        public string TermsOfService { get; set; }

        [JsonProperty("website")]
        public string Website { get; set; }

        [JsonProperty("caa-identities")]
        public string[] CaaIdentities { get; set; }
    }
}