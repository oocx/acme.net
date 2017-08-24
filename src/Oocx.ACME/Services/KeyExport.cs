using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Oocx.Asn1PKCS;
using Oocx.Asn1PKCS.Asn1BaseTypes;
using Oocx.Asn1PKCS.PKCS1;

namespace Oocx.ACME.Services
{
    public class KeyExport
    {
        private readonly string basePath;

        public KeyExport(string basePath)
        {
            this.basePath = basePath ?? throw new ArgumentNullException(nameof(basePath));
        }

        public enum Format
        {
            [Obsolete]
            DotNetXml = 1,
            PEM       = 2,
            DER       = 3
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

            var pem = keyBytes.EncodeAsPEM(PEMExtensions.RSAPrivateKey);

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