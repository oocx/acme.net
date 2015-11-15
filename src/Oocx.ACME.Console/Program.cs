using System;
using System.IO;
using System.Linq;
using System.Net;
using Oocx.ACME.Client;
using Oocx.ACME.Services;
using Oocx.Asn1PKCS;
using Oocx.Asn1PKCS.Asn1BaseTypes;
using Oocx.Asn1PKCS.PKCS10;

namespace Oocx.ACME.Console
{
    public class Program
    {
        public void Main(string[] args)
        {
            try
            {
                ServicePointManager.ServerCertificateValidationCallback +=
                    (sender, cert, chain, sslPolicyErrors) => true;

                //const string staging = "https://acme-staging.api.letsencrypt.org";
                const string production = "https://acme-v01.api.letsencrypt.org";

                var client = new AcmeClient(production);
                //var registration = client.RegisterAsync().Result;

                //var registration = client.UpdateRegistrationAsync("https://acme-staging.api.letsencrypt.org/acme/reg/30716").Result;
                var registration =
                    client.UpdateRegistrationAsync("https://acme-v01.api.letsencrypt.org/acme/reg/12661").Result;
                System.Console.WriteLine(registration.Agreement);
                System.Console.WriteLine(string.Join(", ", registration.Contact));
                System.Console.WriteLine(registration.Certificates);

                var authorization = client.NewDnsAuthorizationAsync("test.startliste.info").Result;

                for (int i = 0; i < authorization.Combinations.Length; i++)
                {
                    System.Console.WriteLine("Challenge combination " + (i + 1) + ":");
                    var combination = authorization.Combinations[i];
                    foreach (int idx in combination)
                    {
                        var challenge = authorization.Challenges[idx];
                        System.Console.WriteLine(challenge.Type + " " + challenge.Uri + " " + challenge.Token + " " +
                                                 challenge.Status);
                    }
                }

                var simpleHttp = authorization.Challenges.First(c => c.Type == "simpleHttp");

                var result = client.AcceptSimpleHttpChallengeAsync(simpleHttp).Result;

                var keyManager = new KeyManager();
                var rsa = keyManager.GetOrCreateKey("test.startliste.info");
                var key = rsa.ExportParameters(true);

                var data = new CertificateRequestData("test.startliste.info", key)
                {
                    C = "DE",
                    S = "NRW",
                    L = "Werther",
                    O = "Aero Club Bünde",
                    OU = ""
                };

                var serializer = new Asn1Serializer();
                var sut = new CertificateRequestAsn1DEREncoder(serializer);
                var csr = sut.EncodeAsDER(data);

                var response = client.NewCertificateRequestAsync(csr).Result;
                System.Console.WriteLine(response.Location);
                System.Console.WriteLine(response.Link);
                File.WriteAllBytes(@"c:\github\acme.net\test.startliste.info.cer", response.Certificate);

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

        private static void PrintError(AcmeException ex)
        {
            System.Console.WriteLine("Fehler:");
            System.Console.WriteLine(ex.Problem.Type);
            System.Console.WriteLine(ex.Problem.Detail);
        }
    }
}
