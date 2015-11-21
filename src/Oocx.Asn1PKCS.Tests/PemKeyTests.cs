using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using FluentAssertions;
using Oocx.Asn1PKCS.Asn1BaseTypes;
using Oocx.Asn1PKCS.Parser;
using Oocx.Asn1PKCS.PKCS1;
using Xunit;

namespace Oocx.Asn1PKCS.Tests
{
    public class PemKeyTests
    {
        const string TestPrivateKey =
            "-----BEGIN RSA PRIVATE KEY-----\n" +
            "MIIEpAIBAAKCAQEAsrBGTeMdHhMbOWSUh4kbp9eZ6LnHTQNHi8ka8MQTeUNwSSVo\n" +
            "5gVGPkpwg+wMioJH0ftdjwo8YHgkjusQSyj9yrrZ/pWAoKhxLXmn7j9zx5swkasd\n" +
            "mVwZEMZ0zma2OFdu+g+BDVAJcn3XY/yYg2WWEdXuE7uJLUNByW2aJHPbDrJeunsm\n" +
            "90GMm9s17s42XJQq/I6KqU8WQoiGQzEiCCQ+HiKE+dGpueoMcnXc1DBEn3y7taQE\n" +
            "rvrQwvs8WqFfPSBw2RMp59LxGXwG6rsHYxGQ6c2FwQfVz7VFSTX1QNMw/siN04Nj\n" +
            "1q40rI8HcGf3kXO1hmj4HQkL960ye2d7OVutqwIDAQABAoIBAEdIpBtRRZ3oUF2C\n" +
            "fvBc5Y/9JjSd3GCgNMwSk7FF/2DKYwKDLohIxV93MdCm+8/LrlwZ85ZrJm/zrd3n\n" +
            "722Ucj6McQerW4978v6JwFD+tjEEe4k5Iss53vP+v71RY1Mxqh+OzNRjQAmRIzqL\n" +
            "m8obXhVKazoq/8vzP0zccPgm0dZX7iAKORGMEl8r7Pg1PVE5CRjzt2/5v/8kct1s\n" +
            "33B+qekyQTqI9EJFUc5EXU0umecfIODgre6R/fJLpqR9Sme14DA7evsabP69ONaC\n" +
            "7QxMHCs5tZ7lTEwz27ZWau07f/3UOYLluap+mwU9hJFIx+oHqOq884aqV/YgQ04y\n" +
            "QWN4ZFECgYEAyiH5ruq7skp3sVcnN68jnnV67Fzdt5CAIt6wGkNfcERFkv8WDS41\n" +
            "+k+TWc61WVipOYcsg1e3JK+FOXxKKFEoow1rbKHctSr8ZGZqNUBsYaEiEd2d9R77\n" +
            "J6YrRY2HBp11dhUQrnIMfI/PRvWOYayRsD9VHSXIT8IeZ1qVtZq3dnsCgYEA4k7g\n" +
            "1uEUmPXqlXARM1y/VcH4iRZ70WWZWZPehKxcorrHD7qQ2l+YhCK8fCRrB/ga8bHS\n" +
            "hrMMQ/ZsMhK2YaXhglRe5ENeMmcU53fMnf3+RS4HEhAfmcENntoBoZvNj6XhGVQ5\n" +
            "m4GpW0nFY47qKTZq/ij5YrykDS/pILaX25/ZFpECgYBwjobI/nFSkOEh4ZQ3D9HX\n" +
            "1TeEExprs42G3VZW6ZllnL7ZYi5XXJo2LsWwxsh5XkOtgzYoVVnolcyr/CgXV0o3\n" +
            "satWLLQINk2hYt5VlPBFVULxi+T/sCbPJ1sFOhAsgNX90TEe0uddHX+g1fBZN41R\n" +
            "BZNRzc5Di6b+ipS/Mc5jrQKBgQC3dUo/5+dSuMiarGbFRIK3eq+IMnndnodaXs0c\n" +
            "sL3IDbIhxhR3ctepBp/V45AAvdjZ5lXMbImc7r93OEQxlM/jX5ItaY+LPJVtr2hp\n" +
            "C+Z9HnsM4ZU8h4wLIp//6tYRhV+dsPGiZtJr9rnVo52gG0VRFNZWfKZhKl3K7zrc\n" +
            "CBFsgQKBgQCtkmMDs3+0ZCXm3q+pHDl4oXmhbjFK4AeLeg/2Ma7stetkG3UfnCXd\n" +
            "HisFLhRueWkQvNEf6ogSk8FJ15P9uXb/YmCCeS2nki7avEnSZGXH5dDjfVbDuIh1\n" +
            "AcOwqy5KsCy6n9dI7u7nu1u2l7UMDYNUrjigD09qN6OlqukcjhpWQQ==\n"+ 
            "-----END RSA PRIVATE KEY-----\n";


        [Fact]
        public void Can_read_a_private_key_from_a_PEM_file()
        {
            // Arrange
            var asn1Parser = new Asn1Parser();
            var sut = new RSAPrivateKeyParser(asn1Parser);

            // Act
            var rsa = sut.ParsePem(new MemoryStream(Encoding.ASCII.GetBytes(TestPrivateKey)));

            // Assert
            rsa.Key.Exponent.Should().Equal(1, 0, 1);

            rsa.Key.Modulus.Length.Should().Be(256);            
            rsa.Key.Modulus[0].Should().Be(0xb2);
            rsa.Key.Modulus[255].Should().Be(0xab);

            rsa.Key.P.Length.Should().Be(128);
        }

        [Fact]
        public void Serializing_and_deserializing_a_private_key_should_result_in_equal_keys()
        {
            // Arrange
            var rsa = new RSACryptoServiceProvider(2048);
            var rsaParameters = rsa.ExportParameters(true);
            var asn1Parser = new Asn1Parser();
            var rsaParser = new RSAPrivateKeyParser(asn1Parser);
            var asn1Serializer = new Asn1Serializer();
            var asn1Rsa = new RSAPrivateKey(rsaParameters);

            // Act
            var serializedPEM = asn1Serializer.Serialize(asn1Rsa).ToArray().EncodeAsPEM(PEMExtensions.RSAPrivateKey);
            var parsedRsaKey = rsaParser.ParsePem(new MemoryStream(Encoding.ASCII.GetBytes(serializedPEM)));

            // Assert
            parsedRsaKey.Key.Exponent.Should().Equal(rsaParameters.Exponent);
            parsedRsaKey.Key.Modulus.Should().Equal(rsaParameters.Modulus);
            parsedRsaKey.Key.P.Should().Equal(rsaParameters.P);
            parsedRsaKey.Key.D.Should().Equal(rsaParameters.D);
            parsedRsaKey.Key.DP.Should().Equal(rsaParameters.DP);
            parsedRsaKey.Key.Q.Should().Equal(rsaParameters.Q);
            parsedRsaKey.Key.DQ.Should().Equal(rsaParameters.DQ);
            parsedRsaKey.Key.InverseQ.Should().Equal(rsaParameters.InverseQ);            
        }
    }
}