using System;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Oocx.Asn1PKCS.Asn1BaseTypes;

namespace Oocx.ACME.Jose
{
    public class JWS
    {
        private readonly RSA rsa;

        public JWS(RSA rsa)
        {
            this.rsa = rsa ?? throw new ArgumentNullException(nameof(rsa));
        }

        public JWSMessage Encode<TPayload, THeader>(TPayload payload, THeader protectedHeader)
        {
            var jwk = GetKey();

            var message = new JWSMessage
            {
                Header = new JWSHeader {
                    Key = jwk,
                    Algorithm = "RS256"
                },
                Payload = JsonConvert.SerializeObject(payload).Base64UrlEncoded(),
                Protected = JsonConvert.SerializeObject(protectedHeader).Base64UrlEncoded()
            };

            message.Signature = rsa.SignData(Encoding.ASCII.GetBytes(message.Protected + "." + message.Payload), HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1).Base64UrlEncoded();

            return message;
        }

        public JsonWebKey GetKey()
        {
            var parameters = rsa.ExportParameters(true);

            return new JsonWebKey {
                KeyType = "RSA",
                Exponent = parameters.Exponent.Base64UrlEncoded(),
                Modulus = parameters.Modulus.Base64UrlEncoded(),
            };
        }

        public string GetSha256Thumbprint()
        {
            var key = GetKey();
            var json = "{\"e\":\"" + key.Exponent + "\",\"kty\":\"RSA\",\"n\":\"" + key.Modulus + "\"}";
            var sha256 = SHA256.Create();
            return sha256.ComputeHash(Encoding.UTF8.GetBytes(json)).Base64UrlEncoded();
        }

        public string GetKeyAuthorization(string token)
        {
            return $"{token}.{GetSha256Thumbprint()}";
        }
    }
}