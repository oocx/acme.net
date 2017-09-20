using Newtonsoft.Json;

namespace Oocx.Jose
{
    public class JwsHeader
    {
        public JwsHeader() { }

        public JwsHeader(string algorithm, Jwk key)
        {
            Algorithm = algorithm;
            Key       = key;
        }

        [JsonProperty("alg")]
        public string Algorithm { get; set; }

        [JsonProperty("jwk")]
        public Jwk Key { get; set; }
    }
}