using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Oocx.Acme.Tests.FakeHttp
{
    public class FakeRequestConfiguration
    {
        private readonly FakeHttpMessageHandler fakeHttpMessageHandler;

        public FakeRequestConfiguration(FakeHttpMessageHandler fakeHttpMessageHandler)
        {
            this.fakeHttpMessageHandler = fakeHttpMessageHandler;
            fakeHttpMessageHandler.Enqueue(this);
        }

        public string Uri { get; set; }

        public StringContent Content { get; set; }

        public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();        

        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;

        public FakeRequestConfiguration Returns<T>(T content, string contentType = "application/json")
        {
            Content = new StringContent(JObject.FromObject(content).ToString(), Encoding.UTF8, contentType);
            Content.Headers.ContentType.MediaType = contentType;
            return this;
        }

        public FakeRequestConfiguration HasStatusCode(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
            return this;
        }

        public FakeRequestConfiguration WithHeader(string key, string value)
        {
            Headers[key] = value;
            return this;
        }

        public FakeRequestConfiguration WithNonce(string value)
        {
            Headers["Replay-Nonce"] = value;
            return this;
        }

        public FakeRequestConfiguration RequestTo(string uri)
        {
            return fakeHttpMessageHandler.RequestTo(uri);
        }

        public HttpResponseMessage GetResponseMessage()
        {
            var response = new HttpResponseMessage(StatusCode) { Content = Content};
            foreach (var header in Headers)
            {
                response.Headers.Add(header.Key, header.Value);
            }
            return response;
        }

        public HttpClient GetHttpClient()
        {              
            var client = new HttpClient(fakeHttpMessageHandler) { BaseAddress = new Uri(fakeHttpMessageHandler.BaseAddress, UriKind.Absolute) };
            return client;
        }
    }
}