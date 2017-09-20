using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions.Execution;

namespace Oocx.Acme.Tests.FakeHttp
{
    public class FakeHttpMessageHandler : HttpMessageHandler
    {        
        private readonly Queue<FakeRequestConfiguration> requestConfigurations = new Queue<FakeRequestConfiguration>();

        private readonly List<HttpRequestMessage> actualRequests = new List<HttpRequestMessage>();        

        public string BaseAddress { get; }

        public FakeHttpMessageHandler(string baseAddress)
        {
            this.BaseAddress = baseAddress;
        }

        public FakeRequestConfiguration RequestTo(string uri)
        {
            return new FakeRequestConfiguration(this) { Uri = BaseAddress + uri };
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Content = await PreventDisposeContentWrapper.CreateWrapperAsync(request.Content);

            actualRequests.Add(request);
            
            var expected = requestConfigurations.Dequeue();
            if (!request.RequestUri.Equals(new Uri(expected.Uri, UriKind.RelativeOrAbsolute)))
            {
                throw new AssertionFailedException($"expected request to '{expected.Uri}', but got request to '{request.RequestUri}'");
            }

            return expected.GetResponseMessage();
        }

        public HttpRequestMessage[] ReceivedRequestsTo(string uri)
        {
            var requests = actualRequests.Where(r => r.RequestUri.Equals(new Uri(BaseAddress + uri))).ToArray();
            if (!requests.Any())
            {                
                throw new AssertionFailedException($"expected a request to {uri}, but only got the following requests: {string.Join(", ", actualRequests.Select(r => r.RequestUri.ToString()))}");
            }
            return requests;
        }

        public void Enqueue(FakeRequestConfiguration fakeRequestConfiguration)
        {
            requestConfigurations.Enqueue(fakeRequestConfiguration);
        }
    }
}