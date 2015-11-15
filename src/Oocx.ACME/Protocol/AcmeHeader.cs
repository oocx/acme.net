using Newtonsoft.Json;

namespace Oocx.ACME.Protocol
{
    public class AcmeHeader
    {
        [JsonProperty("nonce")]
        public string Nonce { get; set; }
    }
}