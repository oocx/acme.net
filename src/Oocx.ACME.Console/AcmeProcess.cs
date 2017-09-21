using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Threading.Tasks;

using Oocx.Acme.Protocol;
using Oocx.Acme.Services;
using Oocx.Pkcs;
using Oocx.Pkcs.PKCS10;

using static Oocx.Acme.Logging.Log;

namespace Oocx.Acme.Console
{
    public class AcmeProcess : IAcmeProcess
    {
        private readonly Options options;
        private readonly IChallengeProvider challengeProvider;
        private readonly IServerConfigurationProvider serverConfiguration;
        private readonly IAcmeClient client;
        private readonly IPkcs12 pkcs12;

        private readonly ICertificateRequestAsn1DerEncoder certificateRequestEncoder;

        public AcmeProcess(
            Options options,
            IChallengeProvider challengeProvider, 
            IServerConfigurationProvider serverConfiguration,
            IAcmeClient client, 
            IPkcs12 pkcs12,
            ICertificateRequestAsn1DerEncoder certificateRequestEncoder)
        {
            this.options = options;
            this.challengeProvider = challengeProvider;
            this.serverConfiguration = serverConfiguration;
            this.client = client;
            this.pkcs12 = pkcs12;
            this.certificateRequestEncoder = certificateRequestEncoder;
        }

        public async Task StartAsync()
        {
            IgnoreSslErrors();

            await RegisterWithServer();

            foreach (var domain in options.Domains)
            {
                bool isAuthorized = await AuthorizeForDomain(domain);
                if (!isAuthorized)
                {
                    Error($"authorization for domain {domain} failed");
                    continue;
                }

                var keyPair = GetNewKeyPair();

                var certificateResponse = await RequestCertificateForDomain(domain, keyPair);

                var certificatePath = SaveCertificateReturnedByServer(domain, certificateResponse);

                SaveCertificateWithPrivateKey(domain, keyPair, certificatePath);

                ConfigureServer(domain, certificatePath, keyPair, options.IISWebSite, options.IISBinding);
            }
        }

        private void ConfigureServer(string domain, string certificatePath, RSAParameters key, string siteName, string binding)
        {
            var certificateHash = serverConfiguration.InstallCertificateWithPrivateKey(certificatePath, "my", key);
            serverConfiguration.ConfigureServer(domain, certificateHash, "my", siteName, binding);
        }

        private async Task<CertificateResponse> RequestCertificateForDomain(string domain, RSAParameters key)
        {
            var csr = CreateCertificateRequest(domain, key);
            return await client.NewCertificateRequestAsync(csr);
        }

        private static RSAParameters GetNewKeyPair()
        {
            var rsa = new RSACryptoServiceProvider(2048);

            return rsa.ExportParameters(true);
        }

        private void SaveCertificateWithPrivateKey(string domain, RSAParameters key, string certificatePath)
        {
            Info("generating pfx file with certificate and private key");

            GetPfxPasswordFromUser();

            try
            {
                var pfxPath = Path.Combine(Environment.CurrentDirectory, $"{domain}.pfx");
                pkcs12.CreatePfxFile(key, certificatePath, options.PfxPassword, pfxPath);
                Info($"pfx file saved to {pfxPath}");
            }
            catch (Exception ex)
            {
                Error("could not create pfx file: " + ex);
            }
        }

        private byte[] CreateCertificateRequest(string domain, RSAParameters key)
        {
            var data = new CertificateRequestData(domain, key);
            var csr = certificateRequestEncoder.EncodeAsDer(data);
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

        private async Task<bool> AuthorizeForDomain(string domain)
        {
            var authorization = await client.NewDnsAuthorizationAsync(domain);

            var challenge = await challengeProvider.AcceptChallengeAsync(domain, options.IISWebSite, authorization);
            if (challenge == null)
            {
                return false;
            }

            System.Console.WriteLine(challenge.Instructions);
            if (!options.AcceptInstructions)
            {
                System.Console.WriteLine("Press ENTER to continue");
                System.Console.ReadLine();
            }
            else
            {
                System.Console.WriteLine("Automatically accepting instructions.");
            }
            var challengeResult = await challenge.Complete();
            return "valid".Equals(challengeResult?.Status, StringComparison.OrdinalIgnoreCase);
        }

        private async Task RegisterWithServer()
        {
            // note: the terms of service is automatically populated from directory.meta.terms-of-service when null

            var registration = await client.RegisterAsync(new NewRegistrationRequest { 
                Agreement = options.AcceptTermsOfService ? options.TermsOfServiceUri : null,
                Contact   = new[] { options.Contact }
            });

            Verbose($"Created at: {registration.CreatedAt}");
            Verbose($"Id: {registration.Id}");
            Verbose($"Contact: {string.Join(", ", registration.Contact)}");
            Verbose($"Initial Ip: {registration.InitialIp}");

            if (!string.IsNullOrWhiteSpace(registration.Location) && options.AcceptTermsOfService)
            {
                Info("accepting terms of service");

                if (registration.Agreement != options.TermsOfServiceUri)
                {
                    Error($"Cannot accept terms of service. The terms of service uri is '{registration.Agreement}', expected it to be '{options.TermsOfServiceUri}'.");
                    return;
                }

                await client.UpdateRegistrationAsync(new UpdateRegistrationRequest(
                    location  : registration.Location,
                    agreement : registration.Agreement,
                    contact   : new[] { options.Contact }
                ));
            }
        }

        private void IgnoreSslErrors()
        {
            if (options.IgnoreSSLCertificateErrors)
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) =>
                {
                    if (sslPolicyErrors != SslPolicyErrors.None)
                    {
                        Verbose($"ignoring SSL certificate error: {sslPolicyErrors}");
                    }

                    return true;
                };
            }
        }
    }
}