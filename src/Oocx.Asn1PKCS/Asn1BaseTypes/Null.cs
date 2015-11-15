namespace Oocx.Asn1PKCS.Asn1BaseTypes
{
    public class Null : Asn1Primitive
    {
        public Null() : base(0x05)
        {
            Data = new byte[0];
        }
    }
}