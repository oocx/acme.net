using Newtonsoft.Json;

namespace Oocx.ACME.Jose
{
    public class JWSHeader
    {

        [JsonProperty("alg")]
        public string Algorithm { get; set; }

        [JsonProperty("jwk")]
        public JsonWebKey Key { get; set; }

    }
}