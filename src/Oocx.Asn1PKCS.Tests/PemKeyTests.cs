using System.IO;
using System.Security.Cryptography;
using System.Text;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace Oocx.Pkcs.Tests
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

        const string jwtPrivateKey = @"-----BEGIN RSA PRIVATE KEY-----
MIICWwIBAAKBgQDdlatRjRjogo3WojgGHFHYLugdUWAY9iR3fy4arWNA1KoS8kVw33cJibXr8bvwUAUparCwlvdbH6dvEOfou0/gCFQsHUfQrSDv+MuSUMAe8jzKE4qW+jK+xQU9a03GUnKHkkle+Q0pX/g6jXZ7r1/xAK5Do2kQ+X5xK9cipRgEKwIDAQABAoGAD+onAtVye4ic7VR7V50DF9bOnwRwNXrARcDhq9LWNRrRGElESYYTQ6EbatXS3MCyjjX2eMhu/aF5YhXBwkppwxg+EOmXeh+MzL7Zh284OuPbkglAaGhV9bb6/5CpuGb1esyPbYW+Ty2PC0GSZfIXkXs76jXAu9TOBvD0ybc2YlkCQQDywg2R/7t3Q2OE2+yo382CLJdrlSLVROWKwb4tb2PjhY4XAwV8d1vy0RenxTB+K5Mu57uVSTHtrMK0GAtFr833AkEA6avx20OHo61Yela/4k5kQDtjEf1N0LfI+BcWZtxsS3jDM3i1Hp0KSu5rsCPb8acJo5RO26gGVrfAsDcIXKC+bQJAZZ2XIpsitLyPpuiMOvBbzPavd4gY6Z8KWrfYzJoI/Q9FuBo6rKwl4BFoToD7WIUS+hpkagwWiz+6zLoX1dbOZwJACmH5fSSjAkLRi54PKJ8TFUeOP15h9sQzydI8zJU+upvDEKZsZc/UhT/SySDOxQ4G/523Y0sz/OZtSWcol/UMgQJALesy++GdvoIDLfJX5GBQpuFgFenRiRDabxrE9MNUZ2aPFaFp+DyAe+b4nDwuJaW2LURbr8AEZga7oQj0uYxcYw==
-----END RSA PRIVATE KEY-----";

        [Fact]
        public void Can_read_jwt_base64url_encoded_key()
        {
            var key = RSAPrivateKey.ParsePem(jwtPrivateKey).Key;

            Assert.Equal(@"{
  ""D"": ""D+onAtVye4ic7VR7V50DF9bOnwRwNXrARcDhq9LWNRrRGElESYYTQ6EbatXS3MCyjjX2eMhu/aF5YhXBwkppwxg+EOmXeh+MzL7Zh284OuPbkglAaGhV9bb6/5CpuGb1esyPbYW+Ty2PC0GSZfIXkXs76jXAu9TOBvD0ybc2Ylk="",
  ""DP"": ""ZZ2XIpsitLyPpuiMOvBbzPavd4gY6Z8KWrfYzJoI/Q9FuBo6rKwl4BFoToD7WIUS+hpkagwWiz+6zLoX1dbOZw=="",
  ""DQ"": ""CmH5fSSjAkLRi54PKJ8TFUeOP15h9sQzydI8zJU+upvDEKZsZc/UhT/SySDOxQ4G/523Y0sz/OZtSWcol/UMgQ=="",
  ""Exponent"": ""AQAB"",
  ""InverseQ"": ""Lesy++GdvoIDLfJX5GBQpuFgFenRiRDabxrE9MNUZ2aPFaFp+DyAe+b4nDwuJaW2LURbr8AEZga7oQj0uYxcYw=="",
  ""Modulus"": ""3ZWrUY0Y6IKN1qI4BhxR2C7oHVFgGPYkd38uGq1jQNSqEvJFcN93CYm16/G78FAFKWqwsJb3Wx+nbxDn6LtP4AhULB1H0K0g7/jLklDAHvI8yhOKlvoyvsUFPWtNxlJyh5JJXvkNKV/4Oo12e69f8QCuQ6NpEPl+cSvXIqUYBCs="",
  ""P"": ""8sINkf+7d0NjhNvsqN/NgiyXa5Ui1UTlisG+LW9j44WOFwMFfHdb8tEXp8UwfiuTLue7lUkx7azCtBgLRa/N9w=="",
  ""Q"": ""6avx20OHo61Yela/4k5kQDtjEf1N0LfI+BcWZtxsS3jDM3i1Hp0KSu5rsCPb8acJo5RO26gGVrfAsDcIXKC+bQ==""
}", JsonConvert.SerializeObject(new
            {
                 key.D,
                 key.DP,
                 key.DQ,
                 key.Exponent,
                 key.InverseQ,
                 key.Modulus,
                 key.P,
                 key.Q
            }, Formatting.Indented));
        }

        [Fact]
        public void Can_read_a_private_key_from_a_PEM_file()
        {
            var rsa = RSAPrivateKey.ParsePem(new MemoryStream(Encoding.ASCII.GetBytes(TestPrivateKey)));
            
            // Assert
            rsa.Key.Exponent.Should().Equal(1, 0, 1);
            rsa.Key.Modulus.Length.Should().Be(256);            
            rsa.Key.Modulus[0].Should().Be(0xb2);
            rsa.Key.Modulus[255].Should().Be(0xab);
            rsa.Key.P.Length.Should().Be(128);
        }

        [Fact]
        public void RSAPrivateKey_parse_and_encode_to_pem_should_result_in_equal_keys()
        {
            // TODO this test sometimes has a missing leading '0' byte.

            var rsa = new RSACryptoServiceProvider(2048);
            var rsaParameters = rsa.ExportParameters(true);

            var privateKey = new RSAPrivateKey(rsaParameters);

            var parsedRsaKey = RSAPrivateKey.ParsePem(privateKey.ToPemString());

            Assert.Equal(rsaParameters.Exponent, parsedRsaKey.Key.Exponent);
            Assert.Equal(rsaParameters.Modulus,  parsedRsaKey.Key.Modulus);
            Assert.Equal(rsaParameters.P,        parsedRsaKey.Key.P);
            Assert.Equal(rsaParameters.D,        parsedRsaKey.Key.D);
            Assert.Equal(rsaParameters.DP,       parsedRsaKey.Key.DP);
            Assert.Equal(rsaParameters.Q,        parsedRsaKey.Key.Q);
            Assert.Equal(rsaParameters.DQ,       parsedRsaKey.Key.DQ);
            Assert.Equal(rsaParameters.InverseQ, parsedRsaKey.Key.InverseQ);
        }
    }
}