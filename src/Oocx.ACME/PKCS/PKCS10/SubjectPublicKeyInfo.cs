namespace Oocx.Pkcs.PKCS10
{
    public class SubjectPublicKeyInfo : Sequence
    {
        public SubjectPublicKeyInfo(AlgorithmIdentifier algorithm, BitString subjectPublicKey)
            : base(algorithm, subjectPublicKey)
        {            
        }        
    }
}