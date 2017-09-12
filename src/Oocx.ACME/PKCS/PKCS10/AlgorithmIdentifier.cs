using Oocx.Pkcs.Asn1BaseTypes;

namespace Oocx.Pkcs.PKCS10
{
    public class AlgorithmIdentifier : Sequence
    {
        public AlgorithmIdentifier(string algorithm) : base(new ObjectIdentifier(algorithm), new Null())
        {            
        }    
    }
}