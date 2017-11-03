using System.Linq;

namespace Oocx.Pkcs
{
    /// <summary>
    /// https://msdn.microsoft.com/en-us/library/windows/desktop/bb540792(v=vs.85).aspx
    /// </summary>
    public class BitString : Asn1Object<byte[]>
    {
        public BitString(byte[] data, byte unusedBits = 0) : base(3)
        {
            Data = new byte[] { 0 }.Concat(data).ToArray();
            UnusedBits = unusedBits;
        }

        public byte UnusedBits { get; }
    }
}