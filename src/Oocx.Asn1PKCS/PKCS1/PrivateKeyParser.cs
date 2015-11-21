using System;
using System.IO;
using System.Linq;
using System.Text;
using Oocx.Asn1PKCS.Asn1BaseTypes;
using Oocx.Asn1PKCS.Parser;

namespace Oocx.Asn1PKCS.PKCS1
{
    public class PrivateKeyParser
    {
        private readonly Asn1Parser parser;

        public PrivateKeyParser(Asn1Parser parser)
        {
            this.parser = parser;
        }

        public RSAPrivateKey ParsePem(string pem)
        {
            using (var stream = new MemoryStream(Encoding.ASCII.GetBytes(pem))) { return ParsePem(stream); }
        }
        public RSAPrivateKey ParsePem(Stream input)
        {
            var der = DecodePem(input);
            using (var derStream = new MemoryStream(der))
            {
                //TODO add more validation, ensure that the algorithm used is RSA

                var asn1 = (Sequence)parser.Parse(derStream).First();
                var octet = (OctetString) asn1.Children.Last();
                using (var octetStream = new MemoryStream(octet.UnencodedValue))
                {
                    var rsaParser = new RSAPrivateKeyParser(parser);
                    return rsaParser.ParseDer(octetStream);
                }
            }
        }

        private static byte[] DecodePem(Stream input)
        {
            string[] lines;
            using (var sr = new StreamReader(input))
            {
                lines = sr.ReadToEnd().Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            }
            if (!"-----BEGIN PRIVATE KEY-----".Equals(lines.First()))
            {
                throw new InvalidDataException("A pem private key file should start with -----BEGIN PRIVATE KEY-----");
            }
            if (!"-----END PRIVATE KEY-----".Equals(lines.Last()))
            {
                throw new InvalidDataException("A pem private key file should end with -----END PRIVATE KEY-----");
            }
            var base64 = string.Join("", lines.Skip(1).Take(lines.Length - 2));
            var der = base64.Base64UrlDecode();
            return der;
        }
    }
}