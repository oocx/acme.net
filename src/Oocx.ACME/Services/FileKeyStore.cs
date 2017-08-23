using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using Oocx.Asn1PKCS.PKCS1;
using static Oocx.ACME.Common.Log;

namespace Oocx.ACME.Services
{
    public class FileKeyStore : IKeyStore
    {
        private readonly string basePath;

        public FileKeyStore(string basePath)
        {
            if (string.IsNullOrWhiteSpace(basePath))
            {
                basePath = Directory.GetCurrentDirectory();
            }

            Verbose($"using key base path {basePath}");
            this.basePath = basePath;
        }

        public RSA GetOrCreateKey(string keyName)
        {
            var rsa = new RSACryptoServiceProvider(2048);

            var keyFileName = Path.Combine(basePath, $"{keyName}.pem");

            Debug.WriteLine(keyFileName);

            if (File.Exists(keyFileName))
            {
                Verbose($"using existing key file {keyFileName}");

                var keyXml = File.ReadAllText(keyFileName);

                var privateKey = RSAPrivateKey.ParsePem(keyXml);

                rsa.ImportParameters(privateKey.Key);
            }
            else
            {
                var privateKey = new RSAPrivateKey(rsa.ExportParameters(true));

                Verbose($"writing new key to file {keyFileName}");

                var pemEncodedPrivateKey = privateKey.ToPemString();

                File.WriteAllText(keyFileName, pemEncodedPrivateKey);
            }

            return rsa;
        }
    }
}
