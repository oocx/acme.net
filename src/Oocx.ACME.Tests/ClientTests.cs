using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Security.Cryptography;
using System.Threading.Tasks;
using FluentAssertions;
using Oocx.ACME.Client;
using Oocx.ACME.Protocol;
using WorldDomination.Net.Http;
using Xunit;

namespace Oocx.ACME.Tests
{
    public class ClientTests
    {
        [Fact]
        public async Task Should_get_a_Directory_object_from_the_default_endpoint()
        {
            // Arrange
            var directory = new Directory();
            var response = new HttpResponseMessage(HttpStatusCode.OK) {Content = new ObjectContent<Directory>(directory, new JsonMediaTypeFormatter()) };
            response.Headers.Add("Replay-Nonce", "nonce");
            var handler = new FakeHttpMessageHandler("http://baseaddress/directory", response);            
            var client = new HttpClient(handler) { BaseAddress =  new Uri("http://baseAddress")};
            var sut = new AcmeClient(client, new RSACryptoServiceProvider());

            // Act
            var discoverResponse = await sut.DiscoverAsync();

            // Assert
            discoverResponse.Should().NotBeNull();
        }

        [Fact]
        public async Task Should_post_a_valid_Registration_message()
        {
            // Arrange
            var directory = new Directory() { NewRegistration = new Uri("http://baseaddress/registration")};
            var response1 = new HttpResponseMessage(HttpStatusCode.OK) { Content = new ObjectContent<Directory>(directory, new JsonMediaTypeFormatter()) };
            response1.Headers.Add("Replay-Nonce", "nonce");

            var registration = new RegistrationResponse();
            var response2 = new HttpResponseMessage(HttpStatusCode.OK) {  Content = new ObjectContent<RegistrationResponse>(registration, new JsonMediaTypeFormatter()) };
            response2.Headers.Add("Replay-Nonce", "nonce");

            var handler = new FakeHttpMessageHandler(new Dictionary<string, HttpResponseMessage>()
            {
                {"http://baseaddress/directory", response1 },
                {"http://baseaddress/registration", response2 }
            });

            var client = new HttpClient(handler) { BaseAddress = new Uri("http://baseAddress") };
            var sut = new AcmeClient(client, new RSACryptoServiceProvider());

            // Act
            var registrationResponse =  await sut.RegisterAsync(false);

            // Assert
            registrationResponse.Should().NotBeNull();
        }
    }
}