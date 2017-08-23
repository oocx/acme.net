using System;
using System.IO;
using System.Linq;
using Oocx.Asn1PKCS.Asn1BaseTypes;

namespace Oocx.Asn1PKCS
{
    public static class PEMExtensions
    {
        public const string RSAPrivateKey = "RSA PRIVATE KEY";

        public const string PrivateKey = "PRIVATE KEY";

        public static string EncodeAsPEM(this byte[] data, string type)
        {
            var base64 = Convert.ToBase64String(data);
            string base64Lines = "";
            for (int i = 0; i < base64.Length; i += 64)
            {
                base64Lines += base64.Substring(i, Math.Min(64, base64.Length - i)) + "\n";
            }
            var pem = $"-----BEGIN {type}-----\n{base64Lines}-----END {type}-----";
            return pem;
        }

        public static byte[] DecodePEM(this Stream pem, string type)
        {
            string[] lines;
            using (var sr = new StreamReader(pem))
            {
                lines = sr.ReadToEnd().Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            }
            if (!$"-----BEGIN {type}-----".Equals(lines.First()))
            {
                throw new InvalidDataException($"The PEM file should start with -----BEGIN {type}-----");
            }
            if (!"-----END PRIVATE KEY-----".Equals(lines.Last()))
            {
                throw new InvalidDataException($"The PEM file should end with -----END {type}-----");
            }
            var base64 = string.Join("", lines.Skip(1).Take(lines.Length - 2));
            var der = base64.Base64UrlDecode();
            return der;
        }
    }
}
