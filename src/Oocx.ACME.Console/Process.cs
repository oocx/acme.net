using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Oocx.ACME.Client;
using Oocx.ACME.IIS;
using Oocx.ACME.Protocol;
using Oocx.ACME.Services;
using Oocx.Asn1PKCS.Asn1BaseTypes;
using Oocx.Asn1PKCS.PKCS10;
using Oocx.Asn1PKCS.PKCS12;
using static Oocx.ACME.Common.Log;

namespace Oocx.ACME.Console
{
    public class Process
    {
        private readonly Options options;

        public Process(Options options)
        {
            this.options = options;
        }

        public async Task Start()
        {            
            IgnoreSslErrors();

            var client = CreateAcmeClient();

            await RegisterWithServer(client);

            foreach (var domain in options.Domains)
            {
                bool isAuthorized = await AuthorizeForDomain(client, domain);
                if (!isAuthorized)
                {
                    Error($"authorization for domain {domain} failed");
                    continue;
                }

                var key = GetPrivateKey(domain);

                var certificateResponse = await RequestCertificateForDomain(client, domain, key);

                var certificatePath = SaveCertificateReturnedByServer(domain, certificateResponse);

                SaveCertificateWithPrivateKey(domain, key, certificatePath);

                if ("iis".Equals(options.ServerConfigurationProvider, StringComparison.OrdinalIgnoreCase))
                {
                    InstallCertificateToIis(domain, certificatePath, key, options.IISWebSite, options.IISBinidng);
                }
            }
        }

        private static void InstallCertificateToIis(string domain, string certificatePath, RSAParameters key, string siteName, string binding)
        {
            var installer = new IISCertificateInstaller();            
            var certificateHash = installer.InstallCertificateWithPrivateKey(certificatePath, "my", key);
            installer.ConfigureIis(domain, certificateHash, "my", siteName, binding);
        }

        private AcmeClient CreateAcmeClient()
        {
            var factory = new KeyStoreFactory();
            var keyManager = factory.GetKeyStore(options.AccountKeyContainerLocation);
            var rsa = keyManager.GetOrCreateKey(options.AccountKeyName);
            var client = new AcmeClient(options.AcmeServer, rsa);
            return client;
        }

        private async Task<CertificateResponse> RequestCertificateForDomain(AcmeClient client, string domain, RSAParameters key)
        {
            var csr = CreateCertificateRequest(domain, key);
            return await client.NewCertificateRequestAsync(csr);                        
        }

        private static RSAParameters GetPrivateKey(string domain)
        {
            var keyManager = new FileKeyStore(Environment.CurrentDirectory);
            var rsa = keyManager.GetOrCreateKey(domain);
            var key = rsa.ExportParameters(true);
            return key;
        }

        private void SaveCertificateWithPrivateKey(string domain, RSAParameters key, string certificatePath)
        {
            Info("generating pfx file with certificate and private key");
            GetPfxPasswordFromUser();

            var pfxGenerator = new Pkcs12();
            var pfxPath = Path.Combine(Environment.CurrentDirectory, $"{domain}.pfx");
            pfxGenerator.CreatePfxFile(key, certificatePath, options.PfxPassword, pfxPath);
            Info($"pfx file saved to {pfxPath}");
        }

        private static byte[] CreateCertificateRequest(string domain, RSAParameters key)
        {
            var data = new CertificateRequestData(domain, key);
            var serializer = new Asn1Serializer();
            var sut = new CertificateRequestAsn1DEREncoder(serializer);
            var csr = sut.EncodeAsDER(data);
            return csr;
        }

        private void GetPfxPasswordFromUser()
        {
            System.Console.CursorVisible = false;

            while (string.IsNullOrWhiteSpace(options.PfxPassword))
            {
                System.Console.Write("Enter password for pfx file: ");
                var color = System.Console.ForegroundColor;
                System.Console.ForegroundColor = System.Console.BackgroundColor;
                
                string pass1 = System.Console.ReadLine();
                System.Console.ForegroundColor = color;

                System.Console.Write("Repeat the password: ");
                System.Console.ForegroundColor = System.Console.BackgroundColor;
            
                string pass2 = System.Console.ReadLine();
                System.Console.ForegroundColor = color;

                if (pass1 == pass2)
                {
                    options.PfxPassword = pass1;
                }
                else
                {
                    System.Console.WriteLine("The passwords do not match.");
                }
            }
            System.Console.CursorVisible = true;
        }

        private static string SaveCertificateReturnedByServer(string domain, CertificateResponse response)
        {
            var certificatePath = Path.Combine(Environment.CurrentDirectory, $"{domain}.cer");
            Info($"saving certificate returned by ACME server to {certificatePath}");
            File.WriteAllBytes(certificatePath, response.Certificate);
            return certificatePath;
        }

        private async Task<bool> AuthorizeForDomain(AcmeClient client, string domain)
        {
            var authorization = await client.NewDnsAuthorizationAsync(domain);

            IChallengeProvider provider;
            if ("manual".Equals(options.ChallengeProvider, StringComparison.OrdinalIgnoreCase))
            {
                provider = new ManualChallengeProvider(client);
            } else if ("iis-http-01".Equals(options.ChallengeProvider, StringComparison.OrdinalIgnoreCase))
            {
                provider = new IISChallengeProvider(client);
            }
            else
            {
                Error($"unsupported challenge provider: {options.ChallengeProvider}");
                return false;
            }

            var challenge = await provider.AcceptChallengeAsync(domain, options.IISWebSite, authorization);
            if (challenge == null)
            {
                return false;
            }            

            System.Console.WriteLine(challenge.Instructions);
            System.Console.WriteLine("Press ENTER to continue");
            System.Console.ReadLine();
            var challengeResult = await challenge.Complete();
            return "valid".Equals(challengeResult?.Status, StringComparison.OrdinalIgnoreCase);
        }

        private async Task RegisterWithServer(AcmeClient client)
        {
            var registration = await client.RegisterAsync(options.AcceptTermsOfService);
            Info($"Terms of service: {registration.Agreement}");
            Verbose($"Created at: {registration.CreatedAt}");
            Verbose($"Id: {registration.Id}");
            Verbose($"Contact: {string.Join(", ", registration.Contact)}");
            Verbose($"Initial Ip: {registration.InitialIp}");

            if (!string.IsNullOrWhiteSpace(registration.Location) && options.AcceptTermsOfService)
            {
                Info("accepting terms of service");
                await client.UpdateRegistrationAsync(registration.Location, registration.Agreement);                
            }
        }

        private void IgnoreSslErrors()
        {
            if (options.IgnoreSSLCertificateErrors)
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) =>
                {
                    if (sslPolicyErrors != SslPolicyErrors.None)
                        Verbose($"ignoring SSL certificate error: {sslPolicyErrors}");
                    return true;
                };
            }
        }
    }
}