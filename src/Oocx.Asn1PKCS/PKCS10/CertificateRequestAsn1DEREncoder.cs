using System;
using System.Linq;
using System.Security.Cryptography;
using Oocx.Asn1PKCS.Asn1BaseTypes;
using Oid = Oocx.Asn1PKCS.Asn1BaseTypes.Oid;

namespace Oocx.Asn1PKCS.PKCS10
{
    public class CertificateRequestAsn1DEREncoder : ICertificateRequestAsn1DEREncoder
    {
        private readonly IAsn1Serializer serializer;

        public CertificateRequestAsn1DEREncoder(IAsn1Serializer serializer)
        {
            this.serializer = serializer;
        }

        public CertificationRequest Encode(CertificateRequestData requestData)
        {
            var publicKeyBytes = serializer.Serialize(new Sequence(new Integer(requestData.Key.Modulus), new Integer(requestData.Key.Exponent))).ToArray();

            var certificationRequestInfo = new CertificationRequestInfo(
                new Integer(0),
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
                        new AttributeTypeAndValue(new ObjectIdentifier(Oid.Attribute.CN), new UTF8String(requestData.CN)))),
                new SubjectPublicKeyInfo(
                    new AlgorithmIdentifier(Oid.Algorithm.RSA),
                    new BitString(publicKeyBytes)),
                new ContextSpecific());

            var certificationRequestInfoBytes = serializer.Serialize(certificationRequestInfo).ToArray();

            var rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(requestData.Key);
            var signatureBytes = rsa.SignData(certificationRequestInfoBytes, SHA256.Create());

            return new CertificationRequest(
                certificationRequestInfo,
                new AlgorithmIdentifier(Oid.Algorithm.sha256RSA),
                new BitString(signatureBytes));
        }

        public byte[] EncodeAsDER(CertificateRequestData requestData)
        {
            var asn1 = Encode(requestData);
            var bytes = serializer.Serialize(asn1).ToArray();
            return bytes;
        }

        public string EncodeAsBase64(CertificateRequestData requestData)
        {
            var bytes = EncodeAsDER(requestData);
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
            var bytes = EncodeAsDER(requestData);
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