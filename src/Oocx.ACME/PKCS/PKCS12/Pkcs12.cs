using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Oocx.Pkcs.PKCS12
{
    public class Pkcs12 : IPkcs12
    {        
        public void CreatePfxFile(RSAParameters key, string pathToCertificate, string password, string pathToPfx)
        {
            var csp = new CspParameters {
                KeyContainerName = "oocx-acme-temp"
            };

            var rsa = new RSACryptoServiceProvider(csp);

            rsa.ImportParameters(key);

            var certBytes = File.ReadAllBytes(pathToCertificate);

            var certificate = new X509Certificate2(certBytes,password, X509KeyStorageFlags.Exportable) {
                PrivateKey = rsa
            };           
            
            var pfxBtes = certificate.Export(X509ContentType.Pkcs12, password);
            File.WriteAllBytes(pathToPfx, pfxBtes);
        }
    }
}