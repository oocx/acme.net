namespace Oocx.Pkcs
{
    public class Asn1Null : Asn1Object
    {       
        public Asn1Null() : base(0x05)
        {
            Data = new byte[0];
        }
    }
}