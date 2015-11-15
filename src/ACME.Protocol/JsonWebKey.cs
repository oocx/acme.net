using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace ACME.Protocol
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

    public class JWSHeader
    {

        [JsonProperty("alg")]
        public string Algorithm { get; set; }

        [JsonProperty("jwk")]
        public JsonWebKey Key { get; set; }

    }
    public class JWSMessage
    {
        [JsonProperty("header")]
        public JWSHeader Header { get; set; }

        [JsonProperty("protected")]
        public string Protected { get; set; }

        [JsonProperty("payload")]
        public string Payload { get; set; }

        [JsonProperty("signature")]
        public string Signature { get; set; }        
    }

    // http://tools.ietf.org/html/rfc7515#page-15
    public class JWS
    {
        private readonly RSA rsa;

        public JWS(RSA rsa)
        {
            this.rsa = rsa;
        }

        public JWSMessage Encode<TPayload, THeader>(TPayload payload, THeader protectedHeader)
        {
            var parameters = rsa.ExportParameters(true);
            var jwk = new JsonWebKey()
            {
                KeyType = "RSA",                                
                Exponent = parameters.Exponent.Base64UrlEncoded(),                
                Modulus = parameters.Modulus.Base64UrlEncoded(),                
            };

            var header = new JWSHeader()
            {
                Key = jwk,
                Algorithm = "RS256"
            };

            var message = new JWSMessage
            {
                Header = header,
                Payload = JsonConvert.SerializeObject(payload).Base64UrlEncoded(),
                Protected = JsonConvert.SerializeObject(protectedHeader).Base64UrlEncoded()
            };

            message.Signature = rsa.SignData(Encoding.ASCII.GetBytes(message.Protected + "." + message.Payload), HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1).Base64UrlEncoded();

            return message;
        }
        
    }
}