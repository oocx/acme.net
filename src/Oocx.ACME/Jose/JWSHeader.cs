using Newtonsoft.Json;

namespace Oocx.ACME.Jose
{
    public class JWSHeader
    {
        public JWSHeader() { }

        public JWSHeader(string algorithm, JWK key)
        {
            Algorithm = algorithm;
            Key       = key;
        }

        [JsonProperty("alg")]
        public string Algorithm { get; set; }

        [JsonProperty("jwk")]
        public JWK Key { get; set; }
    }
}