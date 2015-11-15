namespace ACME.Protocol.Asn1
{
    public class Sequence : Asn1Container
    {
        public Sequence(params IAsn1Element[] children) : base(0x30, children)
        {
        }
    }
}