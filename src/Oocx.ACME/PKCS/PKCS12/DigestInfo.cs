using Oocx.Pkcs;

namespace Oocx.Pkcs.PKCS12
{
    public class DigestInfo : Sequence
    {
        public DigestInfo(DigestAlgorithmIdentifier digestAlgorithm, Digest digest) 
            : base(digestAlgorithm, digest)
        {

        }
    }
}