using System;
using System.IO;
using System.Linq;
using FluentAssertions;
using Oocx.ACME.Services;
using Oocx.Asn1PKCS.Asn1BaseTypes;
using Oocx.Asn1PKCS.PKCS10;
using Oocx.Asn1PKCS.PKCS12;
using Xunit;

namespace Oocx.Asn1PKCS.Tests
{
    public class CsrTests
    {
        [Fact]
        public void Should_serialize_a_certificate_signing_request()
        {
            // Arrange

            var keyManager = new KeyStore(Environment.CurrentDirectory);
            var rsa = keyManager.GetOrCreateKey("test.startliste.info");
            var key = rsa.ExportParameters(true);

            var data = new CertificateRequestData("test.startliste.info", key)
            {
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

            // Assert            

            File.WriteAllBytes(@"r:\request.der", der);
            File.WriteAllText(@"r:\request.txt", base64);
            bytes.Should().NotBeNull();

            // openssl req -in r:\request.txt -noout -text
        }

        [Fact]
        public void Convert_xml_key_to_pem()
        {
            // Arrange            
            var keyStore = new KeyStore(Environment.CurrentDirectory);            
            var key = keyStore.GetOrCreateKey("test.startliste.info");
            var sut = new KeyExport(Environment.CurrentDirectory);

            // Act
            sut.Save(key.ExportParameters(true), "test.startliste.info", KeyExport.Format.PEM);

            // Assert
            File.Exists(@"C:\github\ACME.net\test.startliste.info.pem").Should().BeTrue();
        }

        
        //[Fact]        
        public void Create_pfx()
        {
            // Arrange            
            var keyManager = new KeyStore(Environment.CurrentDirectory);
            var key = keyManager.GetOrCreateKey("test.startliste.info").ExportParameters(true);
            var sut = new Pkcs12();
            
            // Act
            sut.CreatePfxFile(key, @"C:\github\ACME.net\test.startliste.info.cer", "test", @"r:\acme\test.startliste.info.pfx");

            // Assert
            File.Exists(@"r:\acme\test.startliste.info.pfx").Should().BeTrue();
        }
    }
}