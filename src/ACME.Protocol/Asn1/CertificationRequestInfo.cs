namespace ACME.Protocol.Asn1
{
    public class CertificationRequestInfo : Sequence
    {
        public CertificationRequestInfo(Integer version, Name subject, SubjectPublicKeyInfo subjectPKInfo, ContextSpecific context) : base(version, subject, subjectPKInfo, context)
        {
            
        }
    }

    public class ContextSpecific : Asn1Primitive
    {
        public ContextSpecific() : base(0xa0)
        {
            Data = new byte[0];
        }
    }
}