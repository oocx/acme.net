using Oocx.Asn1PKCS.Asn1BaseTypes;

namespace Oocx.Asn1PKCS.PKCS10
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