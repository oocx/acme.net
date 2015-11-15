using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using ACME.Protocol.Asn1;

namespace ACME.Protocol
{
    public class KeyManager
    {
        const string keyPath = @"C:\github\ACME.net";

        public RSA GetOrCreateKey(string keyName)
        {
            var rsa = new RSACryptoServiceProvider(2048);
            
            var keyFileName = Path.Combine(keyPath, $"{keyName}.xml");
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

        public void SaveKeyAsPEM(RSAParameters key, string keyName)
        {
            var asn1key = new RSAPrivateKey(key);
            var serializer = new Asn1Serializer();
            var keyBytes = serializer.Serialize(asn1key).ToArray();

            var base64 = Convert.ToBase64String(keyBytes);
            string base64lines = "";
            for (int i = 0; i < base64.Length; i += 64)
            {
                base64lines += base64.Substring(i, Math.Min(64, base64.Length - i)) + "\n";
            }
            var pem = $"-----BEGIN ENCRYPTED PRIVATE KEY-----\n{base64lines}-----END ENCRYPTED PRIVATE KEY-----";

            var keyFileName = Path.Combine(keyPath, $"{keyName}.pem");
            File.WriteAllText(keyFileName, pem);

            keyFileName = Path.Combine(keyPath, $"{keyName}.der");
            File.WriteAllBytes(keyFileName, keyBytes);
        }
    }

    public class Pkcs12
    {        
        public void CreatePfxFile(string keyName, string pathToCertificate, string password, string pathToPfx)
        {
            var keyManager = new KeyManager();
            var rsa = keyManager.GetOrCreateKey(keyName);

            var csp = new CspParameters();
            csp.KeyContainerName = keyName + "-temp";
            var rsa2 = new RSACryptoServiceProvider(csp);
            rsa2.ImportParameters(rsa.ExportParameters(true));

            var certBytes = File.ReadAllBytes(pathToCertificate);
            var certificate = new X509Certificate2(certBytes,password, X509KeyStorageFlags.Exportable) {PrivateKey = rsa2};
            var pfxBtes = certificate.Export(X509ContentType.Pkcs12, password);
            File.WriteAllBytes(pathToPfx, pfxBtes);            

        }
    }
}
