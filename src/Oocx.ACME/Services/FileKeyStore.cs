using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Cryptography;
using Oocx.Asn1PKCS.Asn1BaseTypes;
using Oocx.Asn1PKCS.PKCS1;
using static Oocx.ACME.Common.Log;

namespace Oocx.ACME.Services
{
    public interface IKeyStore
    {
        RSA GetOrCreateKey(string keyName);
    }

    public class KeyStoreFactory
    {
        public IKeyStore GetKeyStore(string storeType)
        {
            if ("user".Equals(storeType) || "machine".Equals(storeType))
            {
                return new KeyContainerStore(storeType);
                
            }            
            return new FileKeyStore(storeType);                            
        }
    }

    public class KeyContainerStore : IKeyStore
    {
        private readonly CspProviderFlags flags;

        public KeyContainerStore(string storeType)
        {
            flags = "machine".Equals(storeType)
                ? CspProviderFlags.UseMachineKeyStore
                : CspProviderFlags.UseUserProtectedKey;

            Verbose($"using key container, flags: {flags}");
        }

        public RSA GetOrCreateKey(string keyName)
        {            
            Verbose($"using key name {keyName}");            
            var csp = new CspParameters()
            {
                KeyContainerName = keyName,
                Flags =  flags,                
            };
                        
            var rsa = new RSACryptoServiceProvider(2048, csp);
            return rsa;
        }
    }

    public class FileKeyStore : IKeyStore
    {
        private readonly string basePath;

        public FileKeyStore() : this(Environment.CurrentDirectory)
        {            
        }

        public FileKeyStore(string basePath)
        {
            if (string.IsNullOrWhiteSpace(basePath))
            {
                basePath = Environment.CurrentDirectory;
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
