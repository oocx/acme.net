using Oocx.Asn1PKCS.Asn1BaseTypes;

namespace Oocx.Asn1PKCS.PKCS10
{
    public class RelativeDistinguishedName : Set
    {
        public RelativeDistinguishedName(params AttributeTypeAndValue[] attributes) : base(attributes)
        {
            
        }
    }
}