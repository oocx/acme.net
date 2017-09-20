using System;
using System.IO;
using System.Linq;

using Oocx.ACME.Services;
using Oocx.Pkcs.PKCS1;
using Oocx.Pkcs.PKCS10;
using Oocx.Pkcs.PKCS12;

using Xunit;

namespace Oocx.Pkcs.Tests
{
    public class CsrTests
    {
        [Fact]
        public void Should_serialize_a_certificate_signing_request()
        {
            /*
            var keyManager = new FileKeyStore(Environment.CurrentDirectory);
            var rsa = keyManager.GetOrCreateKey("test.startliste.info");
            var key = rsa.ExportParameters(true);
            */

            var key = RSAPrivateKey.ParsePem(@"
-----BEGIN RSA PRIVATE KEY-----
MIIEowIBAAKCAQEAq57sJKTDY5K3w9Gf40xDhpHwyOshO2EXEg4doP4tX6+eAHUP
h04Kb3/X2jfgdQHGjTYMyv9Q0AZ5msMOUjxnxbt1GvhVih/yaDOqjysWbBXbRWFZ
5eBk6PHbYmPTVcfE9RyyNS8bp9ykZgJoU0Q1V2UxjQX5JkMJLnFxegKkw/fSZizj
IlUSjnDCP13Gs/cmualyqxlZipsSzaAuasoeIT/qanick3UL+tATIJJ8Zytacntj
Gca9ZMs6xtmvSkumEd2d6pBsIItEwRIG0+9MFb7q7yTySiewFs/+5duEpIL+K83k
IX0nB+15TsjZFnJ+6rLY+YRifjc8vVZHeFD8iwIDAQABAoIBAERetQuqGe7mqc/Y
iH5YSQRoyoh4Z45M0RCP0Azthaz7fRIIkI2iMPUXdKoKHaDveqaR9EnAqfSdx784
WtG3H849rlr2uLkkngEWKCoOC8o2cNq0fEhge0Lz6ybIxw4C3juZ2YLnh/h5JYNA
DUiywR9WgIWCbi3ogdVfO0pUmEg7IG6Q0KLPicabLwcrBKDAloBdS/q7aeFrSEIG
0Hu+CJCKPterdYbFmJ5zHFmOYvToIkX5AYT+G1EfaGx4FHly0wC1eSZIaU/krNd/
U9vdMA2gd8alcG/GwOih5RunIJoGad20hh6gLsw8KHX0s2GDlRvk2Q71cH2YEdo4
SXe7Tw0CgYEA3epZNvO/gZj/YM5ljcn6Hxf2/RscqreH2K4GM0yTp6OEOvm6Dg4t
9JOA+6YxT3Dbf8aEcWDleDcJJY4EfkL4Lqq2dn4vbF+AevVAH1Ml6/ZJwvFuqcMk
9g8FJQqO/Ul/MrFHhi4oXzg3dZPeMKZTN2ACZHiKTwzC0GtCzXdRGVUCgYEAxfsA
1CCUpgsjKvhBxLvpVqIEi5gzMarO7UvgVprvZ/0NFGkgrQC5c3Hhs5mrsf8pB1Gf
EJvTaFGGgr9o2+JzVXs9PJ1njEoelS35PUrHZ8raW2TWJQVAToCfzbylPTY0sJEl
iX++fJJa7AjgYPcriOiZbtBTZHBiqpdAKsC+Pl8CgYEAh25J3BuNuE3jLPVJTOsC
1o8NkRJGwHkZUseByTTmt9w3Crb1MTa+HREYGnwmg9DgZG6GzZrQ8DjGQEEXxOai
B/jvOglwb7co9eFOrM9VyVeZVHt7iecqW3B3N0/mS/XaxtkiSWVKBjKMxhjj9NTM
3HKfgyl9Xxjum7uaHULAH7ECgYATPwJmnMA2oBCMJdQm7umRHXD5rRMU+fjhwqWN
ZcRuRIBYApxFlTNyEJkTX5X8WMTBTGL9N1jG5F4CKd9kuM/jeHaMhPTDA5WThQOc
vL9DzMmLZvMWaDtHJmPimTsrBzD6FTIj+sIm1Ad4uKgvZPfbeFkqF6BzvCUrVkbL
oS8dWwKBgGevHLbGPIeNjrAN3YK3T0VaeB60aigK1PRb+qHL4TDngK2wG6Xy6XVd
iQoWlyFgiNQ/qJcD8dcVwT83KvogfIGb/PLZ9LLRkt4g4ZEKj76gaQIr/U/DVB2I
yXgq378FifWsRemLckRjOC+T0brxqxzqPflbe+c2AHZgMSjuRplC
-----END RSA PRIVATE KEY-----").Key;

            var data = new CertificateRequestData("test.startliste.info", key) {
                C = "DE",
                S = "NRW",
                L = "Werther",
                O = "Aero Club Bünde",
                OU = ""
            };

            var serializer = new Asn1Serializer();
            var sut = new CertificateRequestAsn1DEREncoder(serializer);


            // Act
            var csr = sut.Encode(data);
            var der = sut.EncodeAsDER(data);
            var base64 = sut.EncodeAsBase64(data);
            var bytes = serializer.Serialize(csr).ToArray();

          Assert.Equal(
@"-----BEGIN NEW CERTIFICATE REQUEST-----
MIICZDCCAUwCAQAwHzEdMBsGA1UEAwwUdGVzdC5zdGFydGxpc3RlLmluZm8wggEi
MA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIBAQCrnuwkpMNjkrfD0Z/jTEOGkfDI
6yE7YRcSDh2g/i1fr54AdQ+HTgpvf9faN+B1AcaNNgzK/1DQBnmaww5SPGfFu3Ua
+FWKH/JoM6qPKxZsFdtFYVnl4GTo8dtiY9NVx8T1HLI1Lxun3KRmAmhTRDVXZTGN
BfkmQwkucXF6AqTD99JmLOMiVRKOcMI/Xcaz9ya5qXKrGVmKmxLNoC5qyh4hP+pq
eJyTdQv60BMgknxnK1pye2MZxr1kyzrG2a9KS6YR3Z3qkGwgi0TBEgbT70wVvurv
JPJKJ7AWz/7l24Skgv4rzeQhfScH7XlOyNkWcn7qstj5hGJ+Nzy9Vkd4UPyLAgMB
AAGgADANBgkqhkiG9w0BAQsFAAOCAQEALuD1Xha1+qUH1eiXlMO6xiFUtKPMnwR1
XgYf7OILUnFvG4gdE80clIKR8smLOg59nURhIzHhPRacT5jRmcbl4zruZhL8yCuV
JOacbdoV69iElZ4BODJwHmJPGajcAw89bUFLezPwRflDlVuiw8/ldAQWsyWtnKVI
n9IgTWDEDboUIrUgv+sRwEue+fOCEtVOj1X4Yi0jOCsnihzn0pQNvvU/w9Vpe5Jr
Gm1FyD6z3pdGktxJKW3ns+xYcova+2nQeSbuVFHA/OTmIckrDa87EUJNbNVWLtwo
FXTQmRtze3w5yKOadkSEyr6FG3qq+3IukRgiuxK12SsN7dE0sIO7ow==
-----END NEW CERTIFICATE REQUEST-----".Replace("\r\n", "\n"), base64);
            
            File.WriteAllBytes(@"request.der", der);
            File.WriteAllText(@"request.txt", base64);
            
            // openssl req -in r:\request.txt -noout -text
        }

        [Fact]
        public void Convert_xml_key_to_pem()
        {
            var keyStore = new FileKeyStore(Environment.CurrentDirectory);
            var key = keyStore.GetOrCreateKey("test.startliste.info");
            var sut = new KeyExport(Environment.CurrentDirectory);

            // Act
            sut.Save(key.ExportParameters(true), "test.startliste.info", KeyFormat.Pem);

            Assert.True(File.Exists(Path.Combine(Environment.CurrentDirectory, "test.startliste.info.pem")));
        }

        //[Fact]        
        public void Create_pfx()
        {
            // Arrange            
            var keyManager = new FileKeyStore(Environment.CurrentDirectory);
            var key = keyManager.GetOrCreateKey("test.startliste.info").ExportParameters(true);
            var sut = new Pkcs12();

            // Act
            sut.CreatePfxFile(key, @"test.startliste.info.cer", "test", @"test.startliste.info.pfx");

            Assert.True(File.Exists(@"test.startliste.info.pfx"));
        }
    }
}