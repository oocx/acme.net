using System.Linq;

namespace Oocx.Asn1PKCS.Asn1BaseTypes
{
    /// <summary>
    /// https://msdn.microsoft.com/en-us/library/windows/desktop/bb540792(v=vs.85).aspx
    /// </summary>
    public class BitString : Asn1Primitive<byte[]>
    {
        public BitString(byte[] data, byte unusedBits = 0) : base(3)
        {
            Data = new byte[] { 0 }.Concat(data).ToArray();
            UnusedBits = unusedBits;
            Size = Length + LengthBytes.Length + 1;
        }

        public byte UnusedBits { get; }
    }
}