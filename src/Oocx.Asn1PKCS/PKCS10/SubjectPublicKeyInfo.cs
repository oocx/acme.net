using Oocx.Asn1PKCS.Asn1BaseTypes;

namespace Oocx.Asn1PKCS.PKCS10
{
    public class SubjectPublicKeyInfo : Sequence
    {
        public SubjectPublicKeyInfo(AlgorithmIdentifier algorithm, BitString subjectPublicKey)
            : base(algorithm, subjectPublicKey)
        {            
        }        
    }
}