using System.IO;
using System.Linq;
using System.Text;
using Oocx.Pkcs.Parser;

namespace Oocx.Pkcs
{
    public class PrivateKeyParser
    {
        private readonly Asn1Parser parser;

        internal PrivateKeyParser(Asn1Parser parser)
        {
            this.parser = parser;
        }

        public PrivateKeyParser()
            : this(new Asn1Parser()) { }

        public RSAPrivateKey ParsePem(string pem)
        {
            using (var stream = new MemoryStream(Encoding.ASCII.GetBytes(pem)))
            {
                return ParsePem(stream);
            }
        }
        
        // This function supports both PKCS#1 & PKCS#8 encodings

        public RSAPrivateKey ParsePem(Stream input)
        {
            var der = Pem.Decode(input, Pem.PrivateKey);

            using (var derStream = new MemoryStream(der))
            {
                // TODO add more validation, ensure that the algorithm used is RSA

                var asn1 = (Sequence)parser.Parse(derStream).First();
                var octet = (OctetString)asn1.Children.Last();
                using (var octetStream = new MemoryStream(octet.UnencodedValue))
                {
                    var rsaParser = new RSAPrivateKeyParser(parser);

                    return rsaParser.ParseDer(octetStream);
                }
            }
        }
    }
}