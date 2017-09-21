using System.IO;
using System.Linq;
using System.Text;
using Oocx.Pkcs.Parser;

namespace Oocx.Pkcs
{
    public class PrivateKeyParser
    {
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