namespace ACME.Protocol.Asn1
{
    public class Set : Asn1Container
    {
        public Set(params IAsn1Element[] children) : base(0x31, children)
        {
        }
    }
}