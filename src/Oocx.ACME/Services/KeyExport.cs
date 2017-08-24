using System;
using System.IO;
using System.Security.Cryptography;
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
            var keyBytes = new RSAPrivateKey(key).ToDerBytes();

            var keyFileName = Path.Combine(basePath, $"{keyName}.der");

            File.WriteAllBytes(keyFileName, keyBytes);
        }

        private void SaveAsPEM(RSAParameters key, string keyName)
        {
            var privateKey = new RSAPrivateKey(key);

            var pem = privateKey.ToPemString();

            var keyFileName = Path.Combine(basePath, $"{keyName}.pem");

            File.WriteAllText(keyFileName, pem);
        }
    }
}