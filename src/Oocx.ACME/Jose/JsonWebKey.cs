using Newtonsoft.Json;

namespace Oocx.ACME.Jose
{
    /// <see cref="http://self-issued.info/docs/draft-ietf-jose-json-web-key.html#rfc.section.4"/>    
    public class JsonWebKey
    {        
        [JsonProperty("kty")]
        public string KeyType { get; set; }

        [JsonProperty("kid")]
        public string KeyId { get; set; }

        [JsonProperty("use")]
        public string Use { get; set; }

        [JsonProperty("n")]
        public string Modulus { get; set; }

        [JsonProperty("e")]
        public string Exponent { get; set; }

        [JsonProperty("d")]
        public string D { get; set; }

        [JsonProperty("p")]
        public string P { get; set; }

        [JsonProperty("q")]
        public string Q { get; set; }

        [JsonProperty("dp")]
        public string DP { get; set; }

        [JsonProperty("dq")]
        public string DQ { get; set; }

        [JsonProperty("qi")]
        public string InverseQ { get; set; }

        [JsonProperty("alg")]        
        public string Algorithm { get; set; }

    }


    // Based on: https://tools.ietf.org/html/draft-ietf-jose-json-web-signature-41#appendix-C

    // http://tools.ietf.org/html/rfc7515#page-15
}