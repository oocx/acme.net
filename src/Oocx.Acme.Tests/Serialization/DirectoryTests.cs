using FluentAssertions;
using Newtonsoft.Json;
using Oocx.Acme.Protocol;
using Xunit;

namespace Oocx.Acme.Tests
{
    public class DirectoryTests
    {
        [Fact]
        public void Can_deserialize()
        {
            // Arrange
            const string json = @"{
              ""new-reg"": ""https://example.com/acme/new-reg"",
              ""recover-reg"": ""https://example.com/acme/recover-reg"",
              ""new-authz"": ""https://example.com/acme/new-authz"",
              ""new-cert"": ""https://example.com/acme/new-cert"",
              ""revoke-cert"": ""https://example.com/acme/revoke-cert""
            }";

            // Act
            var directory = JsonConvert.DeserializeObject<Directory>(json);

            // Assert
            directory.NewRegistration.Should().Be("https://example.com/acme/new-reg");
            directory.RecoverRegistration.Should().Be("https://example.com/acme/recover-reg");
            directory.NewAuthorization.Should().Be("https://example.com/acme/new-authz");
            directory.NewCertificate.Should().Be("https://example.com/acme/new-cert");
            directory.RevokeCertificate.Should().Be("https://example.com/acme/revoke-cert");
        }
    }
}
