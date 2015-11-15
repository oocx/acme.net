namespace ACME.Protocol.Asn1
{
    public class SubjectPublicKeyInfo : Sequence
    {
        public SubjectPublicKeyInfo(AlgorithmIdentifier algorithm, BitString subjectPublicKey) : base(algorithm, subjectPublicKey)
        {            
        }        
    }
}