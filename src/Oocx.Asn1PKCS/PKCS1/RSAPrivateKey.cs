using System.Reflection;
using System.Security.Cryptography;
using Oocx.Asn1PKCS.Asn1BaseTypes;

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

        public RSAPrivateKey(Integer modulus, Integer publicExponent, Integer privateExponent, Integer prime1,
            Integer prime2, Integer exponent1, Integer exponent2, Integer coefficient)
            : this(new Integer(0), modulus, publicExponent, privateExponent, prime1, prime2, exponent1, exponent2, coefficient)
        {            
        }

        public RSAPrivateKey(Integer version, Integer modulus, Integer publicExponent, Integer privateExponent, Integer prime1, Integer prime2, Integer exponent1, Integer exponent2, Integer coefficient)
            : base(version, modulus, publicExponent, privateExponent, prime1, prime2, exponent1, exponent2, coefficient)
        {
            Key = new RSAParameters()
            {
                Modulus = modulus.UnencodedValue,
                Exponent = publicExponent.UnencodedValue,
                D = privateExponent.UnencodedValue,
                P = prime1.UnencodedValue,
                Q = prime2.UnencodedValue,
                DP = exponent1.UnencodedValue,
                DQ = exponent2.UnencodedValue,
                InverseQ = coefficient.UnencodedValue
            };
        }
        
        public  RSAParameters Key { get; private set; }
    }
}