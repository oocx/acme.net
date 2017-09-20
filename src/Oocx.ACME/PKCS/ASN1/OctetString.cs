using System;

namespace Oocx.Pkcs
{
    public class OctetString : Asn1Object<byte[]>
    {
        public OctetString(byte[] data) : base(0x04)
        {
            Data = data ?? throw new ArgumentNullException(nameof(data));
            UnencodedValue = data;
        }
    }
}