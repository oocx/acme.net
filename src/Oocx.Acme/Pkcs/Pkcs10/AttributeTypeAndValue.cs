namespace Oocx.Pkcs
{
    public class AttributeTypeAndValue : Sequence
    {
        public AttributeTypeAndValue(ObjectIdentifier type, Asn1Object value) 
            : base(type, value)
        {
            
        }
    }
}