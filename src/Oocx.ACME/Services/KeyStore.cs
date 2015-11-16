using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Oocx.Asn1PKCS.Asn1BaseTypes;
using Oocx.Asn1PKCS.PKCS1;

namespace Oocx.ACME.Services
{
    public class KeyStore
    {
        private readonly string basePath;

        public KeyStore(string basePath)
        {
            this.basePath = basePath;
        }

        public RSA GetOrCreateKey(string keyName)
        {
            var rsa = new RSACryptoServiceProvider(2048);
            
            var keyFileName = Path.Combine(basePath, $"{keyName}.xml");
            Debug.WriteLine(keyFileName);

            if (File.Exists(keyFileName))
            {
                Debug.WriteLine("verwende vorhandene Datei");
                var keyXml = File.ReadAllText(keyFileName);
                rsa.FromXmlString(keyXml);
            }
            else
            {
                var keyXml = rsa.ToXmlString(true);
                Debug.WriteLine(keyXml);
                File.WriteAllText(keyFileName, keyXml);
            }

            return rsa;
        }

        
    }

    public class KeyExport
    {
        private readonly string basePath;

        public KeyExport(string basePath)
        {
            this.basePath = basePath;
        }

        public enum Format
        {
            DotNetXml,
            PEM,
            DER
        }
        public void Save(RSAParameters key, string keyName, Format format)
        {
            switch (format)
            {
                case Format.PEM:
                    SaveAsPEM(key, keyName);
                    break;
                case Format.DER:                
                    SaveAsDER(key, keyName);                
                    break;
                case Format.DotNetXml:                
                    SaveAsXml(key, keyName);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(format), format, null);
            }
        }

        private void SaveAsXml(RSAParameters key, string keyName)
        {
            var keyFileName = Path.Combine(basePath, $"{keyName}.xml");
            var rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(key);
            var xml = rsa.ToXmlString(true);
            File.WriteAllText(keyFileName, xml);
        }

        private void SaveAsDER(RSAParameters key, string keyName)
        {
            var keyBytes = GetKeyAsDER(key);
            var keyFileName = Path.Combine(basePath, $"{keyName}.der");
            File.WriteAllBytes(keyFileName, keyBytes);
        }

        private void SaveAsPEM(RSAParameters key, string keyName)
        {
            var keyBytes = GetKeyAsDER(key);

            var base64 = Convert.ToBase64String(keyBytes);
            string base64Lines = "";
            for (int i = 0; i < base64.Length; i += 64)
            {
                base64Lines += base64.Substring(i, Math.Min(64, base64.Length - i)) + "\n";
            }
            var pem = $"-----BEGIN ENCRYPTED PRIVATE KEY-----\n{base64Lines}-----END ENCRYPTED PRIVATE KEY-----";

            var keyFileName = Path.Combine(basePath, $"{keyName}.pem");
            File.WriteAllText(keyFileName, pem);
        }

        private static byte[] GetKeyAsDER(RSAParameters key)
        {
            var asn1Key = new RSAPrivateKey(key);
            var serializer = new Asn1Serializer();
            var keyBytes = serializer.Serialize(asn1Key).ToArray();
            return keyBytes;
        }
    }
}
