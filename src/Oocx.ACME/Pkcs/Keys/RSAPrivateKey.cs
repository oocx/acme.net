using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Oocx.Pkcs
{
    public class RSAPrivateKey : Sequence
    {
        public RSAPrivateKey(RSAParameters key) :
            this(new DerInteger(key.Modulus), new DerInteger(key.Exponent), new DerInteger(key.D), new DerInteger(key.P), new DerInteger(key.Q), new DerInteger(key.DP), new DerInteger(key.DQ), new DerInteger(key.InverseQ))
        {
            Key = key;
        }

        internal RSAPrivateKey(
            DerInteger modulus, 
            DerInteger publicExponent, 
            DerInteger privateExponent,
            DerInteger prime1,
            DerInteger prime2, 
            DerInteger exponent1,
            DerInteger exponent2,
            DerInteger coefficient)
            : this(new DerInteger(0), modulus, publicExponent, privateExponent, prime1, prime2, exponent1, exponent2, coefficient)
        {
        }

        internal RSAPrivateKey(
            DerInteger version, 
            DerInteger modulus,
            DerInteger publicExponent, 
            DerInteger privateExponent, 
            DerInteger prime1, 
            DerInteger prime2, 
            DerInteger exponent1, 
            DerInteger exponent2, 
            DerInteger coefficient)
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

        private static byte[] AddPadding(byte[] data)
        {
            // As a result of parsing the asn1 Integer, the parser might have removed a leading zero.
            // We need to add it back here, as they are expected by the .NET RsaParameters class

            return (data.Length % 2 == 1)
                ? new byte[] { 0 }.Concat(data).ToArray()
                : data;
        }

        public RSAParameters Key { get; }

        public string ToPemString() => Pem.Encode(ToDerBytes(), Pem.RSAPrivateKey);

        public byte[] ToDerBytes()
        {
            var serializer = new Asn1Serializer();

            return serializer.Serialize(this).ToArray();
        }

        public static RSAPrivateKey ParsePem(string pem)
        {
            var parser = new RSAPrivateKeyParser();

            return parser.ParsePem(pem);
        }

        public void WriteTo(Stream stream, KeyFormat format)
        {
            byte[] keyBytes;

            switch (format)
            {
                case KeyFormat.Der:
                    keyBytes = ToDerBytes();
                    break;
                case KeyFormat.Pem:
                    keyBytes = Encoding.ASCII.GetBytes(ToPemString());
                    break;
                default:
                    throw new Exception("Unexpected key format:" + format);
            }

            stream.Write(keyBytes, 0, keyBytes.Length);
        }
    }
}