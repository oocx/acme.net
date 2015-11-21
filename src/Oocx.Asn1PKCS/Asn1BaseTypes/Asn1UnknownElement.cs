using System;

namespace Oocx.Asn1PKCS.Asn1BaseTypes
{
    public class Asn1UnknownElement : Asn1Primitive<Byte[]>
    {
        public Asn1UnknownElement(byte tag, byte[] data) : base(tag)
        {
            Data = data;
            UnencodedValue = data;
        }
    }
}