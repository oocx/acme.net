using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Oocx.ACME.Client;
using Xunit;

namespace Oocx.ACME.Tests
{
    public class StagingTests
    {
        [Fact]
        public async Task Can_discovery_urls()
        {
            var key = RSA.Create(2048);

            var httpClient = new HttpClient {
                BaseAddress = new Uri("https://acme-staging.api.letsencrypt.org")
            };

            var client = new AcmeClient(httpClient, key);


            var directory = await client.DiscoverAsync();

            Assert.Equal(new Uri("https://acme-staging.api.letsencrypt.org/acme/new-reg"),     directory.NewRegistration);
            Assert.Equal(new Uri("https://acme-staging.api.letsencrypt.org/acme/new-authz"),   directory.NewAuthorization);
            Assert.Equal(new Uri("https://acme-staging.api.letsencrypt.org/acme/new-cert"),    directory.NewCertificate);
            Assert.Equal(new Uri("https://acme-staging.api.letsencrypt.org/acme/revoke-cert"), directory.RevokeCertificate);
            Assert.Equal(new Uri("https://acme-staging.api.letsencrypt.org/acme/key-change"),  directory.KeyChange);

            Assert.Equal("https://letsencrypt.org/documents/LE-SA-v1.1.1-August-1-2016.pdf", directory.Meta.TermsOfService);

            var result = await client.RegisterAsync(
                termsOfServiceUri: null, 
                contact: new[] { "mailto:test@test.com" }
            );
        }
    }
}