namespace Oocx.Asn1PKCS.Asn1BaseTypes
{
    public class OctetString : Asn1Primitive
    {
        public OctetString(byte[] data) : base(0x04)
        {
            Data = data;
        }
    }
}