using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Web.Administration;

namespace Oocx.ACME.IIS
{
    public class IISCertificateInstaller
    {
        private readonly ServerManager manager;

        public IISCertificateInstaller()
        {
            manager = new ServerManager();
        }

        public void InstallCertificate(string domain, X509Certificate2 certificate)
        {
            var site = manager.GetSiteForDomain(domain);
            var binding = site.Bindings.FirstOrDefault(b => string.Equals(domain, b.Host, StringComparison.OrdinalIgnoreCase) && "https".Equals(b.Protocol, StringComparison.OrdinalIgnoreCase));
            
        }
    }
}