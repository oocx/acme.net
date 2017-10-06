using System;
using System.IO;
using System.Text;

namespace Oocx.Pkcs
{
    internal static class Pem
    {
        // Preambles
        public const string RSAPrivateKey = "RSA PRIVATE KEY"; // PKCS#1
        public const string PrivateKey    = "PRIVATE KEY";     // PKCS#8

        public static string Encode(byte[] derBytes, string type)
        {
            var base64String = Convert.ToBase64String(derBytes);

            var pemBuilder = new StringBuilder();

            pemBuilder.AppendFormat("-----BEGIN {0}-----\n", type);

            for (int i = 0; i < base64String.Length; i += 64)
            {
                pemBuilder.Append(base64String.Substring(i, Math.Min(64, base64String.Length - i)));
                pemBuilder.Append("\n");
            }

            pemBuilder.AppendFormat("-----END {0}-----", type);
            
            return pemBuilder.ToString();
        }

        public static byte[] Decode(Stream pem, string type)
        {
            var sb = new StringBuilder();

            string line;

            //  $"-----BEGIN {type}-----"
            //  $"-----END {type}-----"

            using (var sr = new StreamReader(pem))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    if (string.IsNullOrEmpty(line)) continue;

                    if (line.StartsWith("-----") && line.EndsWith("-----")) continue;
                    
                    sb.Append(line);
                }
            }
            
            return sb.ToString().Base64UrlDecode(); // der encoded bytes
        }
    }
}