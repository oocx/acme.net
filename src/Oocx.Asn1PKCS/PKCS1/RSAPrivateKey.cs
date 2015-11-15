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

        }

        public RSAPrivateKey(Integer modulus, Integer publicExponent, Integer privateExponent, Integer prime1, Integer prime2, Integer exponent1, Integer exponent2, Integer coefficient)
            : base(new Integer(0) /*TODO version*/, modulus, publicExponent, privateExponent, prime1, prime2, exponent1, exponent2, coefficient)
        {

        }
    }
}