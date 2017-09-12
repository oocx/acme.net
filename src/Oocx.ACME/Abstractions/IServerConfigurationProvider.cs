using System.Security.Cryptography;

namespace Oocx.ACME.Services
{
    public interface IServerConfigurationProvider
    {
        byte[] InstallCertificateWithPrivateKey(
            string certificatePath, 
            string certificateStoreName,
            RSAParameters privateKey
        );

        void ConfigureServer(
            string domain,
            byte[] certificateHash, 
            string certificateStoreName, 
            string siteName, 
            string binding
        );
    }
}