using Oocx.Asn1PKCS.Asn1BaseTypes;

namespace Oocx.Asn1PKCS.PKCS10
{
    public class CertificationRequestInfo : Sequence
    {
        public CertificationRequestInfo(Integer version, Name subject, SubjectPublicKeyInfo subjectPKInfo, ContextSpecific context) : base(version, subject, subjectPKInfo, context)
        {
            
        }
    }
}