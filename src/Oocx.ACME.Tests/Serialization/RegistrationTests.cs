using System;
using Newtonsoft.Json;
using Oocx.ACME.Jose;
using Oocx.ACME.Protocol;
using Xunit;

namespace Oocx.ACME.Tests
{
    public class RegistrationTests
    {
        [Fact]
        public void Can_serialize()
        {
            var request = new NewRegistrationRequest
            {
                Contact = new[]
                {
                    "mailto:cert-admin@example.com",
                    "tel:+12025551212"
                },
                Key = new JsonWebKey
                {
                    Algorithm = "none"
                },
                Agreement = "https://example.com/acme/terms",
                Authorizations = "https://example.com/acme/reg/1/authz",
                Certificates = "https://example.com/acme/reg/1/cert",

            };

            var json = JsonConvert.SerializeObject(request, Formatting.Indented, new JsonSerializerSettings {
                DefaultValueHandling = DefaultValueHandling.Ignore
            });


            Assert.Equal(@"{
  ""resource"": ""new-reg"",
  ""jwk"": {
    ""alg"": ""none""
  },
  ""contact"": [
    ""mailto:cert-admin@example.com"",
    ""tel:+12025551212""
  ],
  ""agreement"": ""https://example.com/acme/terms"",
  ""authorizations"": ""https://example.com/acme/reg/1/authz"",
  ""certificates"": ""https://example.com/acme/reg/1/cert""
}", json);
            
        }
    }
}
