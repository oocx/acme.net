namespace Oocx.Asn1PKCS.Asn1BaseTypes
{
    public class ContextSpecific : Asn1Primitive
    {
        public ContextSpecific() : base(0xa0)
        {
            Data = new byte[0];
        }
    }
}