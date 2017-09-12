using System;

namespace Oocx.Pkcs.Asn1BaseTypes
{
    public class OctetString : Asn1Primitive<byte[]>
    {
        public OctetString(byte[] data) : base(0x04)
        {
            Data = data ?? throw new ArgumentNullException(nameof(data));
            UnencodedValue = data;
        }
    }
}