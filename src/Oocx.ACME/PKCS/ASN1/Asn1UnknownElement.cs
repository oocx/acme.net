namespace Oocx.Pkcs.Asn1BaseTypes
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