namespace Oocx.Pkcs
{
    public class DigestInfo : Sequence
    {
        public DigestInfo(DigestAlgorithmIdentifier digestAlgorithm, Digest digest) 
            : base(digestAlgorithm, digest)
        {

        }
    }
}