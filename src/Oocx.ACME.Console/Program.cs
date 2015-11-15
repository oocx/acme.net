using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static System.Console;

using CommandLine;
using Oocx.ACME.Client;
using Oocx.ACME.Services;
using Oocx.Asn1PKCS;
using Oocx.Asn1PKCS.Asn1BaseTypes;
using Oocx.Asn1PKCS.PKCS10;
using Oocx.Asn1PKCS.PKCS12;

namespace Oocx.ACME.Console
{
    public class Program
    {
        public void Main(string[] args)
        {            
            Parser.Default.ParseArguments<Options>(args)                
                .WithNotParsed(ArgumentsError)
                .WithParsed(Execute);
        }
      
        private void Execute(Options options)
        {
            try
            {
                ExecuteInternal(options).GetAwaiter().GetResult();
            }
            catch (AggregateException ex)
            {
                var acmeEx = ex.InnerExceptions.OfType<AcmeException>().FirstOrDefault();
                if (acmeEx != null)
                {
                    PrintError(acmeEx);
                }
                else
                {
                    throw;
                }
            }
            catch (AcmeException ex)
            {
                PrintError(ex);
            }
        }

        private async Task ExecuteInternal(Options options)
        {
            if (options.IgnoreSSLCertificateErrors)
            {                
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) =>
                {
                    WriteLine($"ignoring SSL certificate error: {sslPolicyErrors}");
                    return true;
                };                                
            }

            WriteLine($"using server {options.AcmeServer}");
            var client = new AcmeClient(options.AcmeServer);

            WriteLine("creating new registration");
            var registration = await client.RegisterAsync();
            WriteLine($"registration: {registration.Location}");

            if (options.AcceptTermsOfService)
            {
                WriteLine("accepting terms of service");
                registration = await client.UpdateRegistrationAsync(registration.Location);
                WriteLine(registration.Agreement);
                WriteLine(string.Join(", ", registration.Contact));
                WriteLine(registration.Certificates);
            }

            WriteLine($"requesting authorization for dns identifier {options.Domain}");
            var authorization = await client.NewDnsAuthorizationAsync(options.Domain);

            WriteLine("accepting simple http challenge");
            var simpleHttp = authorization.Challenges.First(c => c.Type == "simpleHttp");            
            var result = await client.AcceptSimpleHttpChallengeAsync(simpleHttp);

            var keyManager = new KeyManager();
            var rsa = keyManager.GetOrCreateKey(options.Domain);
            var key = rsa.ExportParameters(true);

            var data = new CertificateRequestData(options.Domain, key);            
            var serializer = new Asn1Serializer();
            var sut = new CertificateRequestAsn1DEREncoder(serializer);
            var csr = sut.EncodeAsDER(data);

            WriteLine("sending certificate request");
            var response = await client.NewCertificateRequestAsync(csr);
            WriteLine(response.Location);
            WriteLine(response.Link);
                        
            var certificatePath = Path.Combine(Environment.CurrentDirectory, $"{options.Domain}.cer");
            WriteLine($"saving certificate returned by ACME server to {certificatePath}");
            File.WriteAllBytes(certificatePath, response.Certificate);

            var pfxPath = Path.Combine(Environment.CurrentDirectory, $"{options.Domain}.pfx");
            WriteLine("generating pfx file with certificate and private key");

            while (string.IsNullOrWhiteSpace(options.PfxPassword))
            {
                Write("Enter password for pfx file: ");
                var color = ForegroundColor;
                ForegroundColor = BackgroundColor;
                CursorVisible = false;
                string pass1 = ReadLine();
                ForegroundColor = color;

                Write("Repeat the password: ");                
                ForegroundColor = BackgroundColor;
                CursorVisible = false;
                string pass2 = ReadLine();
                ForegroundColor = color;

                if (pass1 == pass2)
                {
                    options.PfxPassword = pass1;
                }
                else
                {
                    WriteLine("The passwords do not match.");
                }
            }

            var pfxGenerator = new Pkcs12();
            pfxGenerator.CreatePfxFile(key, certificatePath, options.PfxPassword, pfxPath);
            WriteLine($"pfx file saved to {pfxPath}");
        }

        private void ArgumentsError(IEnumerable<Error> obj)
        {

        }

        private static void PrintError(AcmeException ex)
        {
            WriteLine("error:");
            WriteLine(ex.Problem.Type);
            WriteLine(ex.Problem.Detail);
        }
    }
}

