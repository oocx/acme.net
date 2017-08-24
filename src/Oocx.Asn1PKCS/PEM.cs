using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Oocx.Asn1PKCS.Asn1BaseTypes;

namespace Oocx.Asn1PKCS
{
    public static class PEM
    {
        // Preambles
        public const string RSAPrivateKey = "RSA PRIVATE KEY"; // PKCS#1
        public const string PrivateKey    = "PRIVATE KEY";     // PKCS#8

        internal static string Encode(byte[] derEncodedBytes, string type)
        {
            var base64 = Convert.ToBase64String(derEncodedBytes);

            string base64Lines = "";

            for (int i = 0; i < base64.Length; i += 64)
            {
                base64Lines += base64.Substring(i, Math.Min(64, base64.Length - i)) + "\n";
            }

            var pem = $"-----BEGIN {type}-----\n{base64Lines}-----END {type}-----";

            return pem;
        }

        internal static byte[] Decode(Stream pem, string type)
        {
            var lines = new List<string>();

            string line;

            using (var sr = new StreamReader(pem))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    if (string.IsNullOrEmpty(line)) continue;

                    lines.Add(line);
                }
            }

            if (!$"-----BEGIN {type}-----".Equals(lines[0]))
            {
                throw new InvalidDataException($"The PEM file should start with -----BEGIN {type}-----");
            }

            if (!"-----END PRIVATE KEY-----".Equals(lines[lines.Count - 1]))
            {
                throw new InvalidDataException($"The PEM file should end with -----END {type}-----");
            }
            
            var base64 = string.Join("", lines.Skip(1).Take(lines.Count - 2));
            var der = base64.Base64UrlDecode();
            return der;
        }
    }
}
