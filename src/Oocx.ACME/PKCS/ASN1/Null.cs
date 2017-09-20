namespace Oocx.Pkcs
{
    internal class Null : Asn1Primitive
    {
        public static Null Instance = new Null();
       
        public Null() : base(0x05)
        {
            Data = new byte[0];
        }
    }
}