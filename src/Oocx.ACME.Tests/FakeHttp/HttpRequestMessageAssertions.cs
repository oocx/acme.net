using System;
using System.Net.Http;
using System.Text;
using FluentAssertions;
using Newtonsoft.Json;
using Oocx.ACME.Jose;
using Oocx.Asn1PKCS.Asn1BaseTypes;

namespace Oocx.ACME.Tests.FakeHttp
{
    public static class HttpRequestMessageAssertions
    {
        public static HttpRequestMessage HasContent(this HttpRequestMessage request, Action<HttpContent> contentAssertion)
        {
            contentAssertion(request.Content);
            return request;
        }

        public static HttpRequestMessage HasContent<T>(this HttpRequestMessage request, Action<T> contentAssertion)
        {
            contentAssertion(request.Content.ReadAsAsync<T>().Result);
            return request;
        }

        public static HttpRequestMessage HasJwsPayload<T>(this HttpRequestMessage request, Action<T> contentAssertion)
        {
            var message = request.Content.ReadAsAsync<JWSMessage>().Result;
            var contentJson = Encoding.UTF8.GetString(message.Payload.Base64UrlDecode());
            var content = JsonConvert.DeserializeObject<T>(contentJson);

            contentAssertion(content);
            return request;
        }

        public static HttpRequestMessage HasHeader(this HttpRequestMessage request, string headerName, string headerValue)
        {
            request.Headers.Should().Contain(headerName);
            request.Headers.GetValues(headerName).Should().Contain(headerValue);
            return request;
        }

        public static HttpRequestMessage HasMethod(this HttpRequestMessage request, HttpMethod method)
        {
            request.Method.Should().Be(method);
            return request;
        }
    }
}