namespace Oocx.Pkcs.PKCS10
{
    public class CertificationRequestInfo : Sequence
    {
        internal CertificationRequestInfo(
            DerInteger version,
            Name subject, 
            SubjectPublicKeyInfo subjectPKInfo, 
            ContextSpecific context) : base(version, subject, subjectPKInfo, context)
        {
            
        }
    }
}