namespace Oocx.Pkcs
{
    internal class Asn1UnknownElement : Asn1Object<byte[]>
    {
        public Asn1UnknownElement(byte tag, byte[] data) : base(tag)
        {
            Data = data;
            UnencodedValue = data;
        }
    }
}