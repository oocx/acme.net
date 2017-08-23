using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;

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
            
            var keyFileName = Path.Combine(basePath, $"{keyName}.xml");
            Debug.WriteLine(keyFileName);

            if (File.Exists(keyFileName))
            {
                Verbose($"using existing key file {keyFileName}");
                var keyXml = File.ReadAllText(keyFileName);
                rsa.FromXmlString(keyXml);
            }
            else
            {
                Verbose($"writing new key to file {keyFileName}");

                var keyXml = rsa.ToXmlString(true);                
                File.WriteAllText(keyFileName, keyXml);
            }

            return rsa;
        }
    }
}
