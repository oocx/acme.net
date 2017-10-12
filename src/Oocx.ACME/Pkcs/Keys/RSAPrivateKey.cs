using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Oocx.Pkcs.Parser;

namespace Oocx.Pkcs
{
    public class RSAPrivateKey : Sequence
    {
        public RSAPrivateKey(RSAParameters key) :
            this(
                new DerInteger(key.Modulus), 
                new DerInteger(key.Exponent),
                new DerInteger(key.D),
                new DerInteger(key.P), 
                new DerInteger(key.Q), 
                new DerInteger(key.DP), 
                new DerInteger(key.DQ), 
                new DerInteger(key.InverseQ)
            )
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

        public string ToPemString()
        {
            return Pem.Encode(ToDerBytes(), Pem.RSAPrivateKey);
        }

        public byte[] ToDerBytes()
        {
            return Asn1.Encode(this);            
        }

        public static RSAPrivateKey ParsePem(string pem)
        {
            using (var stream = new MemoryStream(Encoding.ASCII.GetBytes(pem)))
            {
                return ParsePem(stream);
            }
        }

        public static RSAPrivateKey ParsePem(Stream input)
        {
            var der = DecodePem(input);

            using (var derStream = new MemoryStream(der))
            {
                return ParseDer(derStream);
            }
        }

        public static RSAPrivateKey ParseDer(MemoryStream derStream)
        {
            var asn1 = (Sequence)Asn1Parser.Default.Parse(derStream).First();

            var ints = asn1.Children.Cast<DerInteger>().ToArray();

            return new RSAPrivateKey(ints[0], ints[1], ints[2], ints[3], ints[4], ints[5], ints[6], ints[7], ints[8]);
        }

        private static byte[] DecodePem(Stream input)
        {
            return Pem.Decode(input, Pem.RSAPrivateKey);
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