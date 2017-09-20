namespace Oocx.Pkcs.PKCS10
{
    public class AttributeTypeAndValue : Sequence
    {
        public AttributeTypeAndValue(ObjectIdentifier type, Asn1Primitive value) : base(type, value)
        {
            
        }
    }
}