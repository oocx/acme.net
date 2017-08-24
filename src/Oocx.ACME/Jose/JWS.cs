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
        private readonly JWK jwk;

        public JWS(RSA rsa)
        {
            this.rsa = rsa ?? throw new ArgumentNullException(nameof(rsa));

            var publicParameters = rsa.ExportParameters(includePrivateParameters: false);

            this.jwk = new JWK {
                KeyType = "RSA",
                Exponent = publicParameters.Exponent.Base64UrlEncoded(),
                Modulus  = publicParameters.Modulus.Base64UrlEncoded(),
            };
        }

        public JWSMessage Encode<TPayload, THeader>(TPayload payload, THeader protectedHeader)
        {
            var message = new JWSMessage {
                Header    = new JWSHeader(algorithm: "RS256", key: jwk),
                Payload   = JsonConvert.SerializeObject(payload).Base64UrlEncoded(),
                Protected = JsonConvert.SerializeObject(protectedHeader).Base64UrlEncoded()
            };

            message.Signature = rsa.SignData(Encoding.ASCII.GetBytes(message.Protected + "." + message.Payload), HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1).Base64UrlEncoded();

            return message;
        }

        private string GetSha256Thumbprint()
        {
            var json = "{\"e\":\"" + jwk.Exponent + "\",\"kty\":\"RSA\",\"n\":\"" + jwk.Modulus + "\"}";

            var sha256 = SHA256.Create();

            return sha256.ComputeHash(Encoding.UTF8.GetBytes(json)).Base64UrlEncoded();
        }

        public string GetKeyAuthorization(string token)
        {
            return $"{token}.{GetSha256Thumbprint()}";
        }
    }
}