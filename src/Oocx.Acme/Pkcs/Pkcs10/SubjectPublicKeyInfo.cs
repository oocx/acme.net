namespace Oocx.Pkcs
{
    public class SubjectPublicKeyInfo : Sequence
    {
        public SubjectPublicKeyInfo(AlgorithmIdentifier algorithm, BitString subjectPublicKey)
            : base(algorithm, subjectPublicKey) { }        
    }
}