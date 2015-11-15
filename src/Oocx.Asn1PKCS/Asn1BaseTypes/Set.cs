namespace Oocx.Asn1PKCS.Asn1BaseTypes
{
    public class Set : Asn1Container
    {
        public Set(params IAsn1Element[] children) : base(0x31, children)
        {
        }
    }
}