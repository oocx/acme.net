namespace Oocx.Pkcs.Asn1BaseTypes
{
    internal class ContextSpecific : Asn1Primitive
    {
        public ContextSpecific() : base(0xa0)
        {
            Data = new byte[0];
        }
    }
}