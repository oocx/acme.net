using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

using Microsoft.Web.Administration;

using Oocx.Acme.Services;

namespace Oocx.Acme.IIS
{
    public class IISServerConfigurationProvider : IServerConfigurationProvider
    {
        private readonly ServerManager manager;

        public IISServerConfigurationProvider()
        {
            manager = new ServerManager();
        }

        public byte[] InstallCertificateWithPrivateKey(string certificatePath, string certificateStoreName, RSAParameters privateKey)
        {
            var certificateBytes = File.ReadAllBytes(certificatePath);
            var x509 = new X509Certificate2(certificateBytes, (string)null, X509KeyStorageFlags.Exportable | X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet);

            var csp = new CspParameters {
                KeyContainerName = x509.GetCertHashString(),
                Flags = CspProviderFlags.UseMachineKeyStore
            };

            var rsa = new RSACryptoServiceProvider(csp);
            rsa.ImportParameters(privateKey);
            x509.PrivateKey = rsa;

            Log.Info($"Installing certificate private key to localmachine\\{certificateStoreName}, container name {csp.KeyContainerName}");

            InstallCertificateToStore(x509, certificateStoreName);
            return x509.GetCertHash();
        }

        public void ConfigureServer(string domain, byte[] certificateHash, string certificateStoreName, string siteName, string binding)
        {
            Log.Info($"configuring IIS to use the new certificate for {domain}");

            var site = GetWebSite(domain, siteName);

            if (site == null)
            {
                return;
            }

            ConfigureBindings(site, certificateHash, certificateStoreName, binding, domain);
        }

        private Site GetWebSite(string domain, string siteName)
        {
            Site site;
            if (siteName == null)
            {
                site = manager.GetSiteForDomain(domain);
                if (site == null)
                {
                    Log.Error($"IIS Web Site with binding for domain {domain} not found, cannot configure IIS.");
                    return null;
                }
            }
            else
            {
                site = manager.Sites[siteName];
                if (site == null)
                {
                    Log.Error($"IIS Web Site {siteName} not found, cannot configure IIS.");
                    return null;
                }
            }
            return site;
        }

        private void ConfigureBindings(Site site, byte[] certificateHash, string certificateStoreName, string binding, string domain)
        {
            Binding[] httpsBindings;
            if (binding != null)
            {
                httpsBindings = site.Bindings.Where(b => b.BindingInformation == binding).ToArray();
            }
            else
            {
                httpsBindings = site.Bindings
                    .Where(
                        b =>
                            string.Equals(domain, b.Host, StringComparison.OrdinalIgnoreCase) &&
                            "https".Equals(b.Protocol, StringComparison.OrdinalIgnoreCase))
                    .ToArray();
            }

            if (!httpsBindings.Any())
            {
                AddNewHttpsBinding(certificateHash, certificateStoreName, site, domain);
            }
            else
            {
                UpdateExistingBindings(certificateHash, certificateStoreName, site, httpsBindings);
            }
        }

        private void InstallCertificateToStore(X509Certificate2 certificate, string certificateStoreName)
        {
            var hash = certificate.GetCertHashString();

            var store = new X509Store(certificateStoreName, StoreLocation.LocalMachine);
            store.Open(OpenFlags.OpenExistingOnly | OpenFlags.ReadWrite);
            if (store.Certificates.OfType<X509Certificate2>()
                .Any(c =>
                    c.Subject == certificate.Subject &&
                    c.HasPrivateKey &&
                    string.Equals(c.GetCertHashString(), hash, StringComparison.OrdinalIgnoreCase)))
            {
                Log.Info($"the certificate with subject {certificate.Subject} and hash {hash} is already installed in store LocalMachine\\{certificateStoreName}");
                store.Close();
                return;
            }

            Log.Info($"Installing certificate with subject {certificate.Subject} and hash {hash} to store LocalMachine\\{certificateStoreName}");

            store.Add(certificate);
            store.Close();
        }

        private void UpdateExistingBindings(byte[] certificateHash, string certificateStoreName, Site site, IEnumerable<Binding> httpsBindings)
        {
            foreach (var binding in httpsBindings)
            {
                Log.Info($"updating existing binding {binding.BindingInformation} in site {site.Name}");
                binding.CertificateHash = certificateHash;
                binding.CertificateStoreName = certificateStoreName;
            }

            manager.CommitChanges();
        }

        private void AddNewHttpsBinding(byte[] certificateHash, string certificateStoreName, Site site, string domain)
        {
            var bindingInformation = $"*:443:{domain}";
            Log.Info($"adding new binding {bindingInformation} to site {site.Name}");
            var binding = site.Bindings.Add(bindingInformation, certificateHash, certificateStoreName);
            binding.SetAttributeValue("sslFlags", 1);
            manager.CommitChanges();
        }
    }
}