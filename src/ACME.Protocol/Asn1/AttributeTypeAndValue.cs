namespace ACME.Protocol.Asn1
{
    public class AttributeTypeAndValue : Sequence
    {
        public AttributeTypeAndValue(ObjectIdentifier type, Asn1Primitive value) : base(type, value)
        {
            
        }
    }
}