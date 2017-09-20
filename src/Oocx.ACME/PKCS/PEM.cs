using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Oocx.Pkcs
{
    internal static class Pem
    {
        // Preambles
        public const string RSAPrivateKey = "RSA PRIVATE KEY"; // PKCS#1
        public const string PrivateKey    = "PRIVATE KEY";     // PKCS#8

        public static string Encode(byte[] derBytes, string type)
        {
            var base64 = Convert.ToBase64String(derBytes);

            var base64Lines = new StringBuilder();

            for (int i = 0; i < base64.Length; i += 64)
            {
                base64Lines.Append(base64.Substring(i, Math.Min(64, base64.Length - i)));
                base64Lines.Append("\n");
            }

            var pem = $"-----BEGIN {type}-----\n{base64Lines.ToString()}-----END {type}-----";

            return pem;
        }

        public static byte[] Decode(Stream pem, string type)
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

            if (!$"-----END {type}-----".Equals(lines[lines.Count - 1]))
            {
                throw new InvalidDataException($"The PEM file should end with -----END {type}-----");
            }
            
            string base64 = string.Join("", lines.Skip(1).Take(lines.Count - 2));

            return base64.Base64UrlDecode(); // der encoded bytes
        }
    }
}