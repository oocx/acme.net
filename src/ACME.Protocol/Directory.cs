using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace ACME.Protocol
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

    public class RegistrationRequest
    {
        [JsonProperty("jwk")]
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

    public class AuthorizationRequest
    {
        [JsonProperty("resource")]
        public string Resource { get; set; }

        [JsonProperty("identifier")]
        public Identifier Identifier { get; set; }
    }

    public class AuthorizationResponse
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("expires")]
        public string Expires { get; set; }

        [JsonProperty("identifier")]
        public Identifier Identifier { get; set; }

        [JsonProperty("challenges")]
        public Challange[] Challenges { get; set; }

        [JsonProperty("combinations")]
        public int[][] Combinations { get; set; }

        [JsonIgnore]
        public Uri Location { get; set; }
    }

    public class Challange
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("uri")]
        public Uri Uri { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("tls")]
        public bool Tls { get; set; }

        [JsonProperty("validationRecord")]
        public ValidationRecord[] ValidationRecord { get; set; }
    }

    public class ChallangeRequest
    {
        [JsonProperty("resource")]
        public string Resource => "challenge";

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("tls")]
        public bool Tls { get; set; }
    }

    public class ValidationRecord
    {
        [JsonProperty("addressResolved")]
        public string AddressResolved { get; set; }

        [JsonProperty("addressUsed")]
        public string AddressUsed { get; set; }

        [JsonProperty("hostname")]
        public string HostName { get; set; }

        [JsonProperty("port")]
        public int Port { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }
    }

    public class Identifier
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }

    public class SimpleHttpChallenge
    {
        [JsonProperty("type")]
        public string Type => "simpleHttp";

        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("tls")]
        public bool Tls { get; set; }
    }

    public class CertificateRequest
    {
        [JsonProperty("resource")]
        public string Resource => "new-cert";

        [JsonProperty("csr")]
        public string Csr { get; set; }         
    }

    public class CertificateResponse
    {
        public string Location { get; set; }

        public string Link { get; set; }

        public byte[] Certificate { get; set; }
    }

    public class Problem
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("detail")]
        public string Detail { get; set; }
    }

    public class AcmeException : Exception
    {
        public Problem Problem { get; }

        public AcmeException(Problem problem) : base($"{problem.Type}: {problem.Detail}")
        {
            Problem = problem;
        }
    }
}
