using System;
using System.IO;
using System.Linq;
using System.Text;
using Oocx.Pkcs.Parser;

namespace Oocx.Pkcs
{
    public static class Pkcs8
    {
        public const string Prefix = "-----BEGIN PRIVATE KEY-----";
        public const string Suffix = "-----END PRIVATE KEY-----";

        public static RSAPrivateKey ParsePem(string pem)
        {
            if (pem == null)
            {
                throw new ArgumentNullException(nameof(pem));
            }

            using (var stream = new MemoryStream(Encoding.ASCII.GetBytes(pem)))
            {
                return ParsePem(stream);
            }
        }
        
        // Parses a PKCS#8 encoded private key
        public static RSAPrivateKey ParsePem(Stream input)
        {
            var der = Pem.Decode(input, Pem.PrivateKey);

            using (var derStream = new MemoryStream(der))
            {
                // TODO add more validation, ensure that the algorithm used is RSA

                var asn1 = (Sequence)Asn1Parser.Default.Parse(derStream).First();
                var octet = (OctetString)asn1.Children.Last();
                using (var octetStream = new MemoryStream(octet.UnencodedValue))
                {
                    return RSAPrivateKey.ParseDer(octetStream);
                }
            }
        }
    }
}

// ref: https://en.wikipedia.org/wiki/PKCS_8
