using System;
using System.Linq;
using System.Security.Cryptography;

namespace Oocx.Pkcs.PKCS10
{
    public class CertificateRequestAsn1DEREncoder : ICertificateRequestAsn1DerEncoder
    {
        private readonly Asn1Serializer serializer;

        public CertificateRequestAsn1DEREncoder(Asn1Serializer serializer)
        {
            this.serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        }

        public CertificationRequest Encode(CertificateRequestData requestData)
        {
            var publicKeyBytes = serializer.Serialize(new Sequence(new DerInteger(requestData.Key.Modulus), new DerInteger(requestData.Key.Exponent))).ToArray();

            var certificationRequestInfo = new CertificationRequestInfo(
                new DerInteger(0),
                new Name(
                    /*new RelativeDistinguishedName(
                        new AttributeTypeAndValue(new ObjectIdentifier(Oid.Attribute.C),
                            new PrintableString(requestData.C))),
                    new RelativeDistinguishedName(
                        new AttributeTypeAndValue(new ObjectIdentifier(Oid.Attribute.S), new UTF8String(requestData.S))),
                    new RelativeDistinguishedName(
                        new AttributeTypeAndValue(new ObjectIdentifier(Oid.Attribute.L), new UTF8String(requestData.L))),
                    new RelativeDistinguishedName(
                        new AttributeTypeAndValue(new ObjectIdentifier(Oid.Attribute.O), new UTF8String(requestData.O))),
                    new RelativeDistinguishedName(
                        new AttributeTypeAndValue(new ObjectIdentifier(Oid.Attribute.OU), new UTF8String(requestData.OU))),*/
                    new RelativeDistinguishedName(
                        new AttributeTypeAndValue(new ObjectIdentifier(Oids.Attribute.CN), new UTF8String(requestData.CN)))),
                new SubjectPublicKeyInfo(
                    new AlgorithmIdentifier(Oids.Algorithm.RSA),
                    new BitString(publicKeyBytes)),
                new ContextSpecific());

            var certificationRequestInfoBytes = serializer.Serialize(certificationRequestInfo).ToArray();

            var rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(requestData.Key);
            var signatureBytes = rsa.SignData(certificationRequestInfoBytes, SHA256.Create());

            return new CertificationRequest(
                certificationRequestInfo : certificationRequestInfo,
                signatureAlgorithm       : new AlgorithmIdentifier(Oids.Algorithm.Sha256RSA),
                signature                : new BitString(signatureBytes)
            );
        }

        public byte[] EncodeAsDer(CertificateRequestData requestData)
        {
            var asn1 = Encode(requestData);
            var bytes = serializer.Serialize(asn1).ToArray();
            return bytes;
        }

        public string EncodeAsBase64(CertificateRequestData requestData)
        {
            var bytes = EncodeAsDer(requestData);
            var base64 = Convert.ToBase64String(bytes);
            string base64lines = "";
            for (int i = 0; i < base64.Length; i += 64)
            {
                base64lines += base64.Substring(i, Math.Min(64, base64.Length - i)) + "\n";
            }
            return $"-----BEGIN NEW CERTIFICATE REQUEST-----\n{base64lines}-----END NEW CERTIFICATE REQUEST-----";
        }
        public string EncodeAsBase64Url(CertificateRequestData requestData)
        {
            var bytes = EncodeAsDer(requestData);
            var base64 = bytes.Base64UrlEncoded();
            string base64lines = "";
            for (int i = 0; i < base64.Length; i += 64)
            {
                base64lines += base64.Substring(i, Math.Min(64, base64.Length - i)) + "\n";
            }
            return $"-----BEGIN NEW CERTIFICATE REQUEST-----\r\n{base64lines}-----END NEW CERTIFICATE REQUEST-----";
        }

    }
}