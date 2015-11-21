using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Web.Administration;
using static Oocx.ACME.Common.Log;

namespace Oocx.ACME.IIS
{
    public class IISCertificateInstaller
    {
        private readonly ServerManager manager;

        public IISCertificateInstaller()
        {
            manager = new ServerManager();
        }

        public void InstallCertificate(string domain, X509Certificate2 certificate, string certificateStoreName)
        {
            InstallCertificateToStore(certificate, certificateStoreName);

            var site = manager.GetSiteForDomain(domain);
            var httpsBindings = site.Bindings
                .Where(b => string.Equals(domain, b.Host, StringComparison.OrdinalIgnoreCase) && "https".Equals(b.Protocol, StringComparison.OrdinalIgnoreCase))
                .ToArray();          
             
            if (!httpsBindings.Any())
            {                
                AddNewHttpsBinding(certificate, certificateStoreName, site, domain);
            }
            else
            {
                UpdateExistingBinding(certificate, certificateStoreName, site, httpsBindings);
            }
        }

        private void InstallCertificateToStore(X509Certificate2 certificate, string certificateStoreName)
        {
            var hash = certificate.GetCertHashString();

            var store = new X509Store(certificateStoreName, StoreLocation.LocalMachine);            
            store.Open(OpenFlags.OpenExistingOnly | OpenFlags.ReadWrite); 
            if (store.Certificates.OfType<X509Certificate2>()
                .Any(
                    c =>
                        c.Subject == certificate.Subject &&
                        c.HasPrivateKey &&                        
                        string.Equals(c.GetCertHashString(), hash, StringComparison.OrdinalIgnoreCase)))
            {
                Info($"the certificate with subject {certificate.Subject} and hash {hash} is already installed in store LocalMachine\\{certificateStoreName}");
                store.Close();
                return;
            }

            Info($"Installing certificate with subject {certificate.Subject} and hash {hash} is already to store LocalMachine\\{certificateStoreName}");
            
            store.Add(certificate);
            store.Close();
        }

        private void UpdateExistingBinding(X509Certificate2 certificate, string certificateStoreName, Site site, IEnumerable<Binding> httpsBindings)
        {
            foreach (var httpsBinding in httpsBindings)
            {
                Info($"updating existing binding {httpsBinding.BindingInformation} in site {site.Name}");
                httpsBinding.CertificateHash = certificate.GetCertHash();
                httpsBinding.CertificateStoreName = certificateStoreName;                
            }            

            manager.CommitChanges();
        }

        private void AddNewHttpsBinding(X509Certificate2 certificate, string certificateStoreName, Site site, string domain)
        {
            var bindingInformation = $"*:443:{domain}";
            Info($"adding new binding {bindingInformation} to site {site.Name}");
            var binding = site.Bindings.Add(bindingInformation, certificate.GetCertHash(), certificateStoreName);
            binding.SetAttributeValue("sslFlags", 1);
            manager.CommitChanges();
        }
    }
}