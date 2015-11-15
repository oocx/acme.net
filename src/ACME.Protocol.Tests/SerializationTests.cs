using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace ACME.Protocol.Tests
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public class SerializationTests
    {
        [Fact]
        public void Should_deserialize_Directory_message()
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
