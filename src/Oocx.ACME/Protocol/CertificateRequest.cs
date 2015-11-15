using Newtonsoft.Json;

namespace Oocx.ACME.Protocol
{
    public class CertificateRequest
    {
        [JsonProperty("resource")]
        public string Resource => "new-cert";

        [JsonProperty("csr")]
        public string Csr { get; set; }         
    }
}