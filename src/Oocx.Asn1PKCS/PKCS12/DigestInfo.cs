using Oocx.Asn1PKCS.Asn1BaseTypes;

namespace Oocx.Asn1PKCS.PKCS12
{
    public class DigestInfo : Sequence
    {
        public DigestInfo(DigestAlgorithmIdentifier digestAlgorithm, Digest digest) : base(digestAlgorithm, digest)
        {

        }
    }
}