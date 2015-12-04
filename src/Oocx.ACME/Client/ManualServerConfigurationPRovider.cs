using System;
using System.Security.Cryptography;
using Oocx.ACME.Common;
using Oocx.ACME.Services;

namespace Oocx.ACME.Client
{
    public class ManualServerConfigurationProvider : IServerConfigurationProvider
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