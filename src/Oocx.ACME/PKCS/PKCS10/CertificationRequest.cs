namespace Oocx.Pkcs.PKCS10
{
    public class CertificationRequest : Sequence
    {
        public CertificationRequest(
            CertificationRequestInfo certificationRequestInfo,
            AlgorithmIdentifier signatureAlgorithm,
            BitString signature)
            : base(certificationRequestInfo, signatureAlgorithm, signature)
        {
        }
    }
}