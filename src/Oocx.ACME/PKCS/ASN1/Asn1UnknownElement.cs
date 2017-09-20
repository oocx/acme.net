namespace Oocx.Pkcs
{
    internal class Asn1UnknownElement : Asn1Primitive<byte[]>
    {
        public Asn1UnknownElement(byte tag, byte[] data) : base(tag)
        {
            Data = data;
            UnencodedValue = data;
        }
    }
}