using Oocx.Pkcs.Asn1BaseTypes;

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