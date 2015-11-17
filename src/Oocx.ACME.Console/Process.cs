using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Oocx.ACME.Client;
using Oocx.ACME.Protocol;
using Oocx.ACME.Services;
using Oocx.Asn1PKCS.Asn1BaseTypes;
using Oocx.Asn1PKCS.PKCS10;
using Oocx.Asn1PKCS.PKCS12;

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
                await AuthorizeForDomain(client, domain);

                var key = GetPrivateKey(domain);

                var certificateResponse = await RequestCertificateForDomain(client, domain, key);

                var certificatePath = SaveCertificateReturnedByServer(domain, certificateResponse);

                SaveCertificateWithPrivateKey(domain, key, certificatePath);
            }
        }

        private AcmeClient CreateAcmeClient()
        {
            var factory = new KeyStoreFactory();
            var keyManager = factory.GetKeyStore(options.AccountKeyContainerType);
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
            System.Console.WriteLine("generating pfx file with certificate and private key");
            GetPfxPasswordFromUser();

            var pfxGenerator = new Pkcs12();
            var pfxPath = Path.Combine(Environment.CurrentDirectory, $"{domain}.pfx");
            pfxGenerator.CreatePfxFile(key, certificatePath, options.PfxPassword, pfxPath);
            System.Console.WriteLine($"pfx file saved to {pfxPath}");
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
            while (string.IsNullOrWhiteSpace(options.PfxPassword))
            {
                System.Console.Write("Enter password for pfx file: ");
                var color = System.Console.ForegroundColor;
                System.Console.ForegroundColor = System.Console.BackgroundColor;
                System.Console.CursorVisible = false;
                string pass1 = System.Console.ReadLine();
                System.Console.ForegroundColor = color;

                System.Console.Write("Repeat the password: ");
                System.Console.ForegroundColor = System.Console.BackgroundColor;
                System.Console.CursorVisible = false;
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
        }

        private static string SaveCertificateReturnedByServer(string domain, CertificateResponse response)
        {
            var certificatePath = Path.Combine(Environment.CurrentDirectory, $"{domain}.cer");
            System.Console.WriteLine($"saving certificate returned by ACME server to {certificatePath}");
            File.WriteAllBytes(certificatePath, response.Certificate);
            return certificatePath;
        }

        private static async Task AuthorizeForDomain(AcmeClient client, string domain)
        {
            var authorization = await client.NewDnsAuthorizationAsync(domain);

            var simpleHttp = authorization.Challenges.First(c => c.Type == "simpleHttp");
            var challenge = await client.AcceptSimpleHttpChallengeAsync(domain, simpleHttp);
            System.Console.WriteLine(challenge.Instructions);
            System.Console.WriteLine("Press ENTER when your server is ready to serve the file");
            System.Console.ReadLine();
            challenge.Complete();
        }

        private async Task RegisterWithServer(AcmeClient client)
        {
            var registration = await client.RegisterAsync(options.AcceptTermsOfService);

            if (!string.IsNullOrWhiteSpace(registration.Location) && options.AcceptTermsOfService)
            {
                System.Console.WriteLine("accepting terms of service");
                registration = await client.UpdateRegistrationAsync(registration.Location);
                System.Console.WriteLine(registration.Agreement);
                System.Console.WriteLine(string.Join(", ", registration.Contact));
                System.Console.WriteLine(registration.Certificates);
            }
        }

        private void IgnoreSslErrors()
        {
            if (options.IgnoreSSLCertificateErrors)
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) =>
                {
                    if (sslPolicyErrors != SslPolicyErrors.None)
                        System.Console.WriteLine($"ignoring SSL certificate error: {sslPolicyErrors}");
                    return true;
                };
            }
        }
    }
}