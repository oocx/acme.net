using System.IO;
using System.Security.Cryptography;

using Oocx.Pkcs;

namespace Oocx.Acme.Services
{
    public class FileKeyStore : IKeyStore
    {
        private readonly string basePath;

        public FileKeyStore(string basePath)
        {
            this.basePath = !string.IsNullOrWhiteSpace(basePath)
                ? basePath
                : Directory.GetCurrentDirectory();

            Log.Verbose($"using key base path {this.basePath}");
        }

        public RSA GetOrCreateKey(string keyName)
        {
            var rsa = new RSACryptoServiceProvider(2048);

            var keyFileName = Path.Combine(basePath, $"{keyName}.pem");

            if (File.Exists(keyFileName))
            {
                Log.Verbose($"using existing key file {keyFileName}");

                var keyXml = File.ReadAllText(keyFileName);

                var privateKey = RSAPrivateKey.ParsePem(keyXml);

                rsa.ImportParameters(privateKey.Key);
            }
            else
            {
                var privateKey = new RSAPrivateKey(rsa.ExportParameters(true));

                Log.Verbose($"writing new key to file {keyFileName}");

                var pemEncodedPrivateKey = privateKey.ToPemString();

                File.WriteAllText(keyFileName, pemEncodedPrivateKey);
            }

            return rsa;
        }
    }
}