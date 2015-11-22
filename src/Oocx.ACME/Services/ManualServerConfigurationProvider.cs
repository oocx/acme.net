using System.Security.Cryptography;
using static  Oocx.ACME.Common.Log;

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