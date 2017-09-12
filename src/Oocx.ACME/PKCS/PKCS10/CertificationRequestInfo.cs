using Oocx.Pkcs.Asn1BaseTypes;

namespace Oocx.Pkcs.PKCS10
{
    public class CertificationRequestInfo : Sequence
    {
        internal CertificationRequestInfo(
            Integer version,
            Name subject, 
            SubjectPublicKeyInfo subjectPKInfo, 
            ContextSpecific context) : base(version, subject, subjectPKInfo, context)
        {
            
        }
    }
}