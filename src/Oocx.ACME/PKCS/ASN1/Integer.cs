using System;
using System.Linq;

namespace Oocx.Asn1PKCS.Asn1BaseTypes
{
    /// <see cref="https://msdn.microsoft.com/en-us/library/windows/desktop/bb540806(v=vs.85).aspx"/>
    public class Integer : Asn1Primitive<byte[]>
    {
        public Integer(int value) : base(2)
        {
            UnencodedValue = BitConverter.GetBytes(value);

            if (value <= byte.MaxValue)
            {
                Data = AddLeadingZero(new[] { (byte)value });
                return;
            }
            var bytes = BitConverter.GetBytes(value);
            if (value < 256 * 256)
            {
                Data = AddLeadingZero(new[] { bytes[1], bytes[0] });
                return;
            }
            if (value < 256 * 256 * 256)
            {
                Data = AddLeadingZero(new[] { bytes[2], bytes[1], bytes[0] });
                return;
            }
            Data = AddLeadingZero(bytes);
        }

        public Integer(byte[] value) : base(2)
        {
            UnencodedValue = value;
            Data = AddLeadingZero(value);
        }

        private byte[] AddLeadingZero(byte[] data)
        {
            if (data.Length == 0 || (data[0] <= 127))
            {
                return data;
            }

            return new byte[] { 0 }.Concat(data).ToArray();
        }
    }
}