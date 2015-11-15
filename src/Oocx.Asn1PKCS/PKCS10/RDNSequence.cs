using Oocx.Asn1PKCS.Asn1BaseTypes;

namespace Oocx.Asn1PKCS.PKCS10
{
    public class RDNSequence : Sequence
    {
        public RDNSequence(params RelativeDistinguishedName[] names) : base(names)
        {
            
        }
    }
}