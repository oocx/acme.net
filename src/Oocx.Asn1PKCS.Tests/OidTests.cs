using Xunit;

namespace Oocx.Pkcs.Tests
{
    public class OidTests
    {
        [Fact]
        public void A()
        {
            Assert.Equal("1.2.840.113549.1.12.10.3", Oids.BagType.CertBag.Value);
        }
    }
}
