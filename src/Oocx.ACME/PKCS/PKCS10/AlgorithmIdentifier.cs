using Oocx.Asn1PKCS.Asn1BaseTypes;

namespace Oocx.Asn1PKCS.PKCS10
{
    public class AlgorithmIdentifier : Sequence
    {
        public AlgorithmIdentifier(string algorithm) : base(new ObjectIdentifier(algorithm), new Null())
        {            
        }    
    }
}