using System;
using System.Text;

namespace ACME.Protocol
{
    public static class UrlSafeBase64Extensions
    {
        public static string Base64UrlEncoded(this byte[] arg)
        {
            string s = Convert.ToBase64String(arg); // Regular base64 encoder
            s = s.Split('=')[0]; // Remove any trailing '='s
            s = s.Replace('+', '-'); // 62nd char of encoding
            s = s.Replace('/', '_'); // 63rd char of encoding
            return s;
        }

        public static byte[] Base64UrlDecode(this string arg)
        {
            string s = arg;
            s = s.Replace('-', '+'); // 62nd char of encoding
            s = s.Replace('_', '/'); // 63rd char of encoding
            switch (s.Length % 4) // Pad with trailing '='s
            {
                case 0: break; // No pad chars in this case
                case 2: s += "=="; break; // Two pad chars
                case 3: s += "="; break; // One pad char
                default:
                    throw new System.Exception(
                        "Illegal base64url string!");
            }
            return Convert.FromBase64String(s); // Standard base64 decoder
        }

        public static string Base64UrlEncoded(this string s)
        {
            s = Encoding.UTF8.GetBytes(s).Base64UrlEncoded();            
            return s;
        }        
    }
}