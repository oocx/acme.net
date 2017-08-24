using System.Linq;
using System.Security.Cryptography;
using Oocx.Asn1PKCS.Asn1BaseTypes;
using Oocx.Asn1PKCS.Parser;

namespace Oocx.Asn1PKCS.PKCS1
{
    public class RSAPrivateKey : Sequence
    {
        // http://blogs.msdn.com/b/shawnfa/archive/2005/11/17/493972.aspx
        public RSAPrivateKey(RSAParameters key) :
            this(new Integer(key.Modulus), new Integer(key.Exponent), new Integer(key.D), new Integer(key.P), new Integer(key.Q), new Integer(key.DP), new Integer(key.DQ), new Integer(key.InverseQ))
        {
            Key = key;
        }

        public RSAPrivateKey(
            Integer modulus, 
            Integer publicExponent, 
            Integer privateExponent,
            Integer prime1,
            Integer prime2, 
            Integer exponent1,
            Integer exponent2,
            Integer coefficient)
            : this(new Integer(0), modulus, publicExponent, privateExponent, prime1, prime2, exponent1, exponent2, coefficient)
        {
        }

        public RSAPrivateKey(Integer version, Integer modulus, Integer publicExponent, Integer privateExponent, Integer prime1, Integer prime2, Integer exponent1, Integer exponent2, Integer coefficient)
            : base(version, modulus, publicExponent, privateExponent, prime1, prime2, exponent1, exponent2, coefficient)
        {
            Key = new RSAParameters {
                Modulus  = AddPadding(modulus.UnencodedValue),
                Exponent = publicExponent.UnencodedValue, // the exponent does not require padding
                D        = AddPadding(privateExponent.UnencodedValue),
                P        = AddPadding(prime1.UnencodedValue),
                Q        = AddPadding(prime2.UnencodedValue),
                DP       = AddPadding(exponent1.UnencodedValue),
                DQ       = AddPadding(exponent2.UnencodedValue),
                InverseQ = AddPadding(coefficient.UnencodedValue)
            };
        }

        private byte[] AddPadding(byte[] data)
        {
            // As a result of parsing the asn1 Integer, the parser might have removed a leading zero.
            // We need to add it back here, as they are expected by the .NET RsaParameters class

            return (data.Length % 2 == 1)
                ? new byte[] { 0 }.Concat(data).ToArray()
                : data;
        }

        public RSAParameters Key { get; private set; }

        public string ToPemString()
        {
            var asn1Serializer = new Asn1Serializer();
            
            return asn1Serializer.Serialize(this).ToArray().EncodeAsPEM(PEMExtensions.RSAPrivateKey);
        }

        public static RSAPrivateKey ParsePem(string pem)
        {
            var asn1Parser = new Asn1Parser();

            var rsaParser = new RSAPrivateKeyParser(asn1Parser);

            return rsaParser.ParsePem(pem);
        }
    }
}