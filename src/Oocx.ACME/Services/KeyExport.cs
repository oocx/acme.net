using System;
using System.IO;
using System.Security.Cryptography;
using Oocx.Pkcs;

namespace Oocx.Acme.Services
{
    public class KeyExport
    {
        private readonly string basePath;

        public KeyExport(string basePath)
        {
            this.basePath = basePath ?? throw new ArgumentNullException(nameof(basePath));
        }

        public void Save(RSAParameters key, string keyName, KeyFormat format)
        {
            var keyFileName = Path.Combine(basePath, $"{keyName}." + GetExtension(format));

            var privateKey = new RSAPrivateKey(key);

            using (var stream = File.OpenWrite(keyFileName))
            {
                privateKey.WriteTo(stream, format);
            }
        }

        private static string GetExtension(KeyFormat format)
        {
            switch (format)
            {
                case KeyFormat.Der : return "der";
                case KeyFormat.Pem : return "pem";
                default            : throw new Exception("Unsupported key format:" + format);
            }
        }
    }
}