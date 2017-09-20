using Newtonsoft.Json;
using Oocx.Acme.Protocol;
using Oocx.Jose;
using Xunit;

namespace Oocx.Acme.Tests
{
    public class RegistrationTests
    {
        [Fact]
        public void Can_serialize()
        {
            var request = new NewRegistrationRequest
            {
                Contact = new[] {
                    "mailto:cert-admin@example.com",
                    "tel:+12025551212"
                },
                Key = new Jwk{
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
}".Replace("\r\n", "\n"), json.Replace("\r\n", "\n"));
            
        }
    }
}
