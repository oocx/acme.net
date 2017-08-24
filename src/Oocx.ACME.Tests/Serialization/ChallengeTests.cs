using System;
using Newtonsoft.Json;
using Oocx.ACME.Protocol;
using Xunit;

namespace Oocx.ACME.Tests
{
    public class ChallengeTests
    {
        [Fact]
        public void Can_deserialize()
        {
            const string json = @"{
    ""type"": ""http-01"",
    ""status"": ""valid"",
    ""token"": ""DGyRejmCefe7v4NfDGDKfA"",
    ""validated"": ""2014-12-01T12:05:00Z"",
    ""keyAuthorization"": ""SXQe-2XODaDxNR...vb29HhjjLPSggwiE""
}";

            var challenge = JsonConvert.DeserializeObject<Challenge>(json);

            Assert.Equal("valid", challenge.Status);
            Assert.Equal("http-01", challenge.Type);
            Assert.Equal(new DateTime(2014, 12, 01, 12, 05, 0, DateTimeKind.Utc), challenge.Validated);
            Assert.Equal("SXQe-2XODaDxNR...vb29HhjjLPSggwiE", challenge.KeyAuthorization);
        }
    }
}
