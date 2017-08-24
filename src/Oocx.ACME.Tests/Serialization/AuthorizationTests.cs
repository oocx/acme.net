using Newtonsoft.Json;
using Oocx.ACME.Protocol;
using Xunit;

namespace Oocx.ACME.Tests
{
    public class AuthorizationTests
    {
        [Fact]
        public void Can_deserialize()
        {
        const string json = @"{
    ""status"": ""valid"",
    ""expires"": ""2015-03-01"",

    ""identifier"": {
        ""type"": ""dns"",
        ""value"": ""example.org""
    },

    ""challenges"": [
        {
            ""type"": ""http-01"",
            ""status"": ""valid"",
            ""validated"": ""2014-12-01T12:05Z"",
            ""keyAuthorization"": ""SXQe-2XODaDxNR...vb29HhjjLPSggwiE""
        }
    ]
}";

            var authorization = JsonConvert.DeserializeObject<AuthorizationResponse>(json);

            Assert.Equal("valid", authorization.Status);
            Assert.Equal("2015-03-01", authorization.Expires);
            Assert.Equal("dns", authorization.Identifier.Type);
            Assert.Equal("example.org", authorization.Identifier.Value);
            Assert.Equal("http-01", authorization.Challenges[0].Type);
        }
    }
}
