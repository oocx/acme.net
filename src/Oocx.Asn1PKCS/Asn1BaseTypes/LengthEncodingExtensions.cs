using System;
using System.Collections.Generic;

namespace Oocx.Asn1PKCS.Asn1BaseTypes
{
    public static class LengthEncodingExtensions
    {
        public static IEnumerable<byte> ToEncodedLength(this int value)
        {
            byte[] bytes;

            if (value <= 127)
            {
                yield return (byte)value;
                yield break;
            }
            if (value <= 255)
            {
                yield return 128 + 1;
                yield return (byte)value;
                yield break;
            }
            if (value <= ushort.MaxValue)
            {
                yield return 128 + 2;
                bytes = BitConverter.GetBytes((ushort)value);
                yield return bytes[1];
                yield return bytes[0];
                yield break;
            }
            if (value < 256 * 256 * 256)
            {
                yield return 128 + 3;
                bytes = BitConverter.GetBytes(value);
                yield return bytes[2];
                yield return bytes[1];
                yield return bytes[0];
                yield break;
            }

            yield return 128 + 4;
            bytes = BitConverter.GetBytes(value);
            yield return bytes[3];
            yield return bytes[2];
            yield return bytes[1];
            yield return bytes[0];
        }

        public static IEnumerable<byte> ToVLQEncodedInt(this int value)
        {
            return ((uint)value).ToVLQEncodedInt();
        }


        /// <summary>
        /// https://en.wikipedia.org/wiki/Variable-length_quantity        
        /// </summary>        
        public static IEnumerable<byte> ToVLQEncodedInt(this uint value)
        {
            if (value < 128)
            {
                yield return (byte)value;
                yield break;
            }

            /*
            	11111111 11111111  11111111  11111111
                aaaabbbb bbbccccc  ccdddddd  deeeeeee
                b = 251658240 + 14680064 = 266338304
                c = 2031616 + 49152 = 2080768
                d = 16128 + 128 = 16256
                e = 128 

            */

            var a = (value & 4026531840) >> 28;
            var b = (value & 266338304) >> 21;
            var c = (value & 2080768) >> 14;
            var d = (value & 16256) >> 7;
            var e = (value & 127);

            if (a > 0) yield return (byte)(a + 128);
            if (a > 0 || b > 0) yield return (byte)(b + 128);
            if (a > 0 || b > 0 || c > 0) yield return (byte)(c + 128);
            if (a > 0 || b > 0 || c > 0 || d > 0) yield return (byte)(d + 128);
            yield return (byte)e;
        }
    }
}