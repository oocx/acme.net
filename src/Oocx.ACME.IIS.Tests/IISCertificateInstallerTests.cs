using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Oocx.Pkcs;
using Oocx.Pkcs.Parser;
using Xunit;

namespace Oocx.Acme.IIS.Tests
{
    public class IISCertificateInstallerTests
    {
        const string TestCertificate =
            "-----BEGIN CERTIFICATE-----\n" +
            "MIIDIDCCAggCCQCbnVO5t9ajFTANBgkqhkiG9w0BAQsFADBSMQswCQYDVQQGEwJE\n" +
            "RTEQMA4GA1UEBwwHV2VydGhlcjENMAsGA1UEAwwEb29jeDEiMCAGCSqGSIb3DQEJ\n" +
            "ARYTbWF0aGlhc0ByYWFja2UuaW5mbzAeFw0xNTExMjExNTI3MDRaFw00MzA0MDcx\n" +
            "NTI3MDRaMFIxCzAJBgNVBAYTAkRFMRAwDgYDVQQHDAdXZXJ0aGVyMQ0wCwYDVQQD\n" +
            "DARvb2N4MSIwIAYJKoZIhvcNAQkBFhNtYXRoaWFzQHJhYWNrZS5pbmZvMIIBIjAN\n" +
            "BgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAz4uVd0fc+3dFOXdfRH2EP8nZ0nld\n" +
            "ZXgt4UJSOD6oOuDsrTXRqdZB/zQjpZJ7q8ZAuiSjs2ZXNyPsbpbQeaeDb0wPrC+7\n" +
            "XlEXY3i3H+ZFrlb9BnmS5qBygnWg8FLLZWEVuPZo4+ADwBFXd8tWsvfLa+ytkOy7\n" +
            "d2zIhM2BpxMYKa5kH2jzE2zopkmMOxDHGOVb4iDvCnFIvHBeT7pUtpEtCHtMgZVB\n" +
            "wosCoBHFo2VC/X/RPyvvlOOjnxTbdZtJ8dCJFfq5i3qPxqDV/fRHiq1sWrVkdCsY\n" +
            "6phZ81wfu5E62ZP7X7Ts4t/BAAoDjJhjD6X6EsBhuOyre7BARqs3q6E/qQIDAQAB\n" +
            "MA0GCSqGSIb3DQEBCwUAA4IBAQA0G76f43CYbzYMLU9RWUnzIi6W7556FZSFMGXv\n" +
            "JVDVHCTa42ESb7ClAYJgx8Ctc8I/j2WOc2tgbm+hHI54g7ZjBkhTZdphlv5rxMQ7\n" +
            "O6/fyRs+4xOMfGH9KqoVsTuZ8zZ+Xb4sXFo9geLELO337o4p5WUHyiN8c3/hOkcl\n" +
            "2ZCq1ZomQCpUBb/YR23r5iJRhogrjvuKxNYeCxxHjq/OSLaQo4rNruiqjUonelC4\n" +
            "N1pwP8l6SKWw7d71AlEQawpy5m5WKwbNJIoNOYOUgTXfPojXo83rLlnRm/jiJtkS\n" +
            "+zhKp9Z2oGROjffzdFkxbnlbOApKBIRQ9u3djrINI8jyc5O3\n" +
            "-----END CERTIFICATE-----\n";


        const string TestPrivateKey =
            "-----BEGIN PRIVATE KEY-----\n" +
            "MIIEvgIBADANBgkqhkiG9w0BAQEFAASCBKgwggSkAgEAAoIBAQDPi5V3R9z7d0U5\n" +
            "d19EfYQ/ydnSeV1leC3hQlI4Pqg64OytNdGp1kH/NCOlknurxkC6JKOzZlc3I+xu\n" +
            "ltB5p4NvTA+sL7teURdjeLcf5kWuVv0GeZLmoHKCdaDwUstlYRW49mjj4APAEVd3\n" +
            "y1ay98tr7K2Q7Lt3bMiEzYGnExgprmQfaPMTbOimSYw7EMcY5VviIO8KcUi8cF5P\n" +
            "ulS2kS0Ie0yBlUHCiwKgEcWjZUL9f9E/K++U46OfFNt1m0nx0IkV+rmLeo/GoNX9\n" +
            "9EeKrWxatWR0KxjqmFnzXB+7kTrZk/tftOzi38EACgOMmGMPpfoSwGG47Kt7sEBG\n" +
            "qzeroT+pAgMBAAECggEANrHLTubyVpbE+HL7+Tm2u3S/YZc4RMBaHu3yav4gKXwT\n" +
            "A0aWnVJojA+6gSzMSKVzc1qqhWGoBFEEQtSd1iGOIquByde+YgmuxzkN0WtG273y\n" +
            "6lzpmLuZ0cWcBxlanrnqFpdhZwQnGNu5GEnO7FUfF68EwpeQg9V4Nn9N9rCygnEI\n" +
            "NfDE8YJS2l4iPmvAb8xdi/dg35VaEXtoFchniNMOl1Fmh7Oi4NT3v1n366YxI1CR\n" +
            "oiZyzRDNwpnbLvXz+Z3PH9UwRHepo4PhQOKuD1eLpuapDS94S2VL5K5nK8WCaV5q\n" +
            "dXGhI+BOub7OP6skZlV6fYl9gcDQ7Pokygek/UuC8QKBgQDxN5GSgZSUAgn3NeEP\n" +
            "XjOP+RGiCo3v0aLbiaFcWLcvbom1F53RattfyS36ok8T0HcnrcmErTQjbJkNJgnr\n" +
            "w2E3xs+0q0GZicPEUA8Os752rURsZSiT0HcR3YqRYRL82nvoSGinMRf+K9UUkZzC\n" +
            "GnfuJGVKIY2SEIYNk/aODvJfrQKBgQDcQ73Hyiqt5ITiGqJ50nzUmzaCPh/jQiUa\n" +
            "hizucmqbojb+KYfRCa1ndKKsbleGcYh6VSjGAvM2cvei9SX6xeUKwmEEHDXY+bW0\n" +
            "LqUZvdhAa8IMh+ULrYgG8MXN0izN8J1ve2Dd7eEKFH/B/Kq58QQ8HnC/SjNZEdQR\n" +
            "WERaFwzvbQKBgQDCBEkhhvpjxfQQFBUvEDz2+4XUSRr7HyoX3O6d4XmOPCGlOBan\n" +
            "P4St2ZoS8wQZ0t5UWvMwtUOvjoTYT5VBPchvXYjpL6o2/dkpiJ9j2u1CdYEIkqD9\n" +
            "q3pqM0BmSKdGG9H39m2+tL0woj7kKW7lrPZz5HBYHP7K0mGQgEea4ajPjQKBgDg4\n" +
            "skBEwhAQvDpaQg6GQ4ag95DW+pyvTXkvtlhGrB99kDvfreMVbUVy5/LLV1vhrsxK\n" +
            "4FVqe1nyjnLxz/fqe0P7yXebG8N+OXr8TPf9FS2cU7OPHE+Ww5nB6ztV4knOmODT\n" +
            "xS3ggghooxOIwqsjcclGm9C3x9N8UXz74rZ8G7khAoGBAOvQKqKZN+Zq6DL03kLq\n" +
            "XvXrs4BZL7X+U4NLSEt+ligSiAFAtyi7XM4zRGK8LHF/C7r4rw4LAcnmIMIh2dsQ\n" +
            "64DrUM7+YDXnk4PRfXjI96dPRwRxpfdWai/RuaTBE/au2x4dcb9IqYNdKhFVPL+w\n" +
            "3tVbPeZ97W7dmw+ueL+4eby5\n" +
            "-----END PRIVATE KEY-----\n";



        // [Fact]        
        public void Should_install_a_x509_certificate_and_update_bindings()
        {
            // Arrange
            var x509 = new X509Certificate2(Encoding.ASCII.GetBytes(TestCertificate), (string)null, X509KeyStorageFlags.Exportable | X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet);            
            var sut = new IISServerConfigurationProvider();
            var privateKey = Pkcs8.ParsePem(TestPrivateKey).Key;    
            
            var csp = new CspParameters {
                KeyContainerName = x509.GetCertHashString(),
                Flags = CspProviderFlags.UseMachineKeyStore
            };

            var rsa2 = new RSACryptoServiceProvider(csp);
            rsa2.ImportParameters(privateKey);
            x509.PrivateKey = rsa2;

            // Act            
            sut.ConfigureServer("test.startliste.info", x509.GetCertHash(), "my", null, null);
        }
    }
}
