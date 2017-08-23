using System.Security.Cryptography;

namespace Oocx.ACME.Services
{
    class ManualServerConfigurationProvider : IServerConfigurationProvider
    {
        public byte[] InstallCertificateWithPrivateKey(string certificatePath, string certificateStoreName, RSAParameters privateKey)
        {
            return null;
        }

        public void ConfigureServer(string domain, byte[] certificateHash, string certificateStoreName, string siteName, string binding)
        {
        }
    }
}