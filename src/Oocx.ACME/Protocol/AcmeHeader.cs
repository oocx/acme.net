using Newtonsoft.Json;

namespace Oocx.Acme.Protocol
{
    public class AcmeHeader
    {
        [JsonProperty("nonce")]
        public string Nonce { get; set; }
    }
}