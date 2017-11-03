using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
using FluentAssertions;

using Oocx.Acme.Protocol;
using Oocx.Acme.Tests.FakeHttp;

using Xunit;

namespace Oocx.Acme.Tests
{
    public class ClientTests
    {
        [Fact]
        public async Task Should_get_a_Directory_object_from_the_default_endpoint()
        {
            var http = new FakeHttpMessageHandler("http://baseaddress/");
            var directory = new Directory();

            var client = 
                http.RequestTo("directory").Returns(directory).WithNonce("nonce")
                .GetHttpClient();

            var sut = new AcmeClient(client, new RSACryptoServiceProvider());

            var discoverResponse = await sut.GetDirectoryAsync();

            discoverResponse.Should().NotBeNull();
        }

        [Fact]
        public async Task Should_post_a_valid_Registration_message()
        {
            var http = new FakeHttpMessageHandler("http://baseaddress/");
            var directory = new Directory { NewRegistration = new Uri("http://baseaddress/registration")};
            var registration = new RegistrationResponse();

            var client = 
                http.RequestTo("directory").Returns(directory).WithNonce("nonce").
                RequestTo("registration").Returns(registration).WithNonce("nonce").
                GetHttpClient();
            
            var sut = new AcmeClient(client, new RSACryptoServiceProvider());

            // Act
            var registrationResponse =  await sut.RegisterAsync(new NewRegistrationRequest {
                Agreement = "agreementUri",
                Contact = new[] { "mailto:admin@example.com" }
            });

            // Assert
            registrationResponse.Should().NotBeNull();
            http.ReceivedRequestsTo("directory").Single().HasMethod(HttpMethod.Get);
            http.ReceivedRequestsTo("registration").Single().HasMethod(HttpMethod.Post).HasJwsPayload<NewRegistrationRequest>(r =>
            {
                r.Agreement.Should().Be("agreementUri");               
                r.Contact.Should().Contain("mailto:admin@example.com");
            });
        }
        
        [Fact]
        public async Task Should_POST_to_get_registration_details_if_the_registration_already_exists()
        {
            var http = new FakeHttpMessageHandler("http://baseaddress/");
            var directory = new Directory { NewRegistration = new Uri("http://baseaddress/registration") };
            var registration = new RegistrationResponse();

            var client =
                http.RequestTo("directory").Returns(directory).WithNonce("nonce").
                RequestTo("registration").Returns(new Problem(), "application/problem+json").HasStatusCode(HttpStatusCode.Conflict).WithHeader("Location", "http://baseaddress/existingreguri").WithNonce("nonce").
                RequestTo("existingreguri").Returns(registration).WithNonce("nonce").
                GetHttpClient();

            var sut = new AcmeClient(client, new RSACryptoServiceProvider());
            
            var registrationResponse = await sut.RegisterAsync(new NewRegistrationRequest {
                Agreement = "agreementUri",
                Contact = new[] { "mailto:admin@example.com" }
            });

            // Assert
            registrationResponse.Should().NotBeNull();
            http.ReceivedRequestsTo("directory").Single().HasMethod(HttpMethod.Get);
            http.ReceivedRequestsTo("registration").Single().HasMethod(HttpMethod.Post).HasJwsPayload<NewRegistrationRequest>(r =>
            {
                r.Agreement.Should().Be("agreementUri");
                r.Contact.Should().Contain("mailto:admin@example.com");
            });

            http.ReceivedRequestsTo("existingreguri").Single().HasMethod(HttpMethod.Post).HasJwsPayload<UpdateRegistrationRequest>(r =>
            {
                r.Agreement.Should().BeNull();
                r.Contact.Should().BeNull();
            });
        }
    }
}