using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oocx.Asn1PKCS
{
    public static class PEMExtensions
    {
        public const string PrivateKey = "RSA PRIVATE KEY";

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
    }
}
