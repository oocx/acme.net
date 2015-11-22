using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Oocx.Asn1PKCS.PKCS12
{
    public class Pkcs12 : IPkcs12
    {        
        public void CreatePfxFile(RSAParameters key, string pathToCertificate, string password, string pathToPfx)
        {
#if DNXCORE50
            throw new PlatformNotSupportedException("pfx export is not supported on core clr");            
#else
            var csp = new CspParameters {KeyContainerName = "oocx-acme-temp"};
            var rsa2 = new RSACryptoServiceProvider(csp);
            rsa2.ImportParameters(key);

            var certBytes = File.ReadAllBytes(pathToCertificate);
            var certificate = new X509Certificate2(certBytes,password, X509KeyStorageFlags.Exportable) {PrivateKey = rsa2};            
            var pfxBtes = certificate.Export(X509ContentType.Pkcs12, password);
            File.WriteAllBytes(pathToPfx, pfxBtes);
#endif
        }
    }
}