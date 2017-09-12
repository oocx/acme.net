using Oocx.Asn1PKCS.Asn1BaseTypes;

namespace Oocx.Asn1PKCS.PKCS10
{
    public class AttributeTypeAndValue : Sequence
    {
        public AttributeTypeAndValue(ObjectIdentifier type, Asn1Primitive value) : base(type, value)
        {
            
        }
    }
}