using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Oocx.Pkcs.Asn1BaseTypes;

namespace Oocx.Pkcs.PKCS1
{
    public class RSAPrivateKey : Sequence
    {
        public RSAPrivateKey(RSAParameters key) :
            this(new Integer(key.Modulus), new Integer(key.Exponent), new Integer(key.D), new Integer(key.P), new Integer(key.Q), new Integer(key.DP), new Integer(key.DQ), new Integer(key.InverseQ))
        {
            Key = key;
        }

        internal RSAPrivateKey(
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

        internal RSAPrivateKey(
            Integer version, 
            Integer modulus,
            Integer publicExponent, 
            Integer privateExponent, 
            Integer prime1, 
            Integer prime2, 
            Integer exponent1, 
            Integer exponent2, 
            Integer coefficient)
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

        public RSAParameters Key { get; private set; }
        
        public string ToPemString()
        {
            return PEM.Encode(this.ToDerBytes(), PEM.RSAPrivateKey);
        }

        public byte[] ToDerBytes()
        {
            var serializer = new Asn1Serializer();

            return serializer.Serialize(this).ToArray();
        }

        public static RSAPrivateKey ParsePem(string pem)
        {
            var rsaParser = new RSAPrivateKeyParser();

            return rsaParser.ParsePem(pem);
        }

        public void WriteTo(Stream stream, KeyFormat format)
        {
            byte[] keyBytes;

            switch (format)
            {
                case KeyFormat.DER:
                    keyBytes = ToDerBytes();
                    break;
                case KeyFormat.PEM:
                    keyBytes = Encoding.ASCII.GetBytes(ToPemString());
                    break;
                default:
                    throw new Exception("Unsupported key format:" + format);
            }

            stream.Write(keyBytes, 0, keyBytes.Length);
        }
    }
}