using System.Security.Cryptography;

namespace Oocx.Pkcs
{
    public class AlgorithmIdentifier : Sequence
    {
        public AlgorithmIdentifier(Oid algorithm)
            : base(new ObjectIdentifier(algorithm), new Asn1Null())
        {            
        }    
    }
}