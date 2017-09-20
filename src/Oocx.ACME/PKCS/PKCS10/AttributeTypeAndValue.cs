namespace Oocx.Pkcs.PKCS10
{
    public class AttributeTypeAndValue : Sequence
    {
        public AttributeTypeAndValue(ObjectIdentifier type, Asn1Object value) : base(type, value)
        {
            
        }
    }
}