using System.IO;
using System.Linq;
using System.Security.Cryptography;
using FluentAssertions;
using Oocx.Pkcs.Parser;
using Xunit;

namespace Oocx.Pkcs.Tests
{
    public class Asn1Tests
    {
        [Fact]
        public void Should_encode_ObjectIdentifiers()
        {
            var oid = new ObjectIdentifier(new Oid("1.3.6.1.4.1.311.21.20"));

            Assert.Equal(new byte[] { 0x2b, 0x06, 0x01, 0x04, 0x01, 0x82, 0x37, 0x15, 0x14 }, oid.Data);
        }

        [Fact] // https://en.wikipedia.org/wiki/Variable-length_quantity
        public void Should_VLQEncode_correctly()
        {
            0.ToVLQEncodedInt().Should().Equal(0x00);
            106903.ToVLQEncodedInt().Should().Equal(134, 195, 23);
            311.ToVLQEncodedInt().Should().Equal(0x82, 0x37);
            0x7F.ToVLQEncodedInt().Should().Equal(0x7F);
            0x80.ToVLQEncodedInt().Should().Equal(0x81, 0x00);
        }

        [Fact]
        public void Should_LengthEncode()
        {
            0.ToEncodedLength().Should().Equal(0);
            127.ToEncodedLength().Should().Equal(127);
            128.ToEncodedLength().Should().Equal(129, 128);
            129.ToEncodedLength().Should().Equal(129, 129);
            255.ToEncodedLength().Should().Equal(129, 255);
            256.ToEncodedLength().Should().Equal(130, 1, 0);
            257.ToEncodedLength().Should().Equal(130, 1, 1);
            511.ToEncodedLength().Should().Equal(130, 1, 255);
            65536.ToEncodedLength().Should().Equal(131, 1, 0, 0);
            16777216.ToEncodedLength().Should().Equal(132, 1, 0, 0, 0);
            16777217.ToEncodedLength().Should().Equal(132, 1, 0, 0, 1);
        }

        [Fact]
        public void Should_encode_UTF8String()
        {
            // Act
            var asn = new UTF8String("Werther");

            // Assert
            Assert.Equal((byte)0x0c, asn.Tag);

            // asn.LengthBytes.Should().Equal(7);
            asn.Data.Should().Equal(0x57, 0x65, 0x72, 0x74, 0x68, 0x65, 0x72);
        }

        [Fact]
        public void Should_serialize_a_sequence()
        {
            // Arrange
            var sequence = new Sequence(new ObjectIdentifier(new Oid("2.5.4.8")), new UTF8String("NRW"));
            
            var bytes = Asn1.Encode(sequence);

            // Assert
            bytes.Should().Equal(0x30, 0x0A, 0x06, 0x03, 0x55, 0x04, 0x08, 0x0C, 0x03, 0x4E, 0x52, 0x57);
        }

        [Fact]
        public void Should_serialize_integer_from_int()
        {
            var ints = new int[] { 0, 127, 128, 256 * 256 };
            var asn1ints = ints.Select(i => new DerInteger(i));

            var bytes = asn1ints.Select(i => Asn1.Encode(i)).ToArray();

            bytes[0].Should().Equal(0x02, 1, 0);
            bytes[1].Should().Equal(0x02, 1, 127);
            bytes[2].Should().Equal(0x02, 2, 0, 128); // da das 1. Bit zur Vorzeichenerkennung genutzt wird, wird bei >= 128 ein 0-Byte voran gestellt
            bytes[3].Should().Equal(0x02, 3, 1, 0, 0);
        }

        [Fact]
        public void Should_parse_integer_to_bytes()
        {
            // Arrange
            var asn1Input = new[]
            {
                new byte[] {2, 1, 0},
                new byte[] {2, 1, 127},
                new byte[] {2, 2, 0, 127},
                new byte[] {2, 2, 0, 128},
                new byte[] {2, 3, 1, 0, 0},
                new byte[] {2, 0},
                new byte[] {2, 7, 0, 165, 163, 214, 2, 169, 62}
            }.Select(b => new MemoryStream(b));

            var expectedParsedValues = new[]
            {
                new byte[] {0},
                new byte[] {127},
                new byte[] {0, 127},
                new byte[] {128},
                new byte[] {1, 0, 0},
                new byte[0],
                new byte[] {165, 163, 214, 2, 169, 62}   //TODO as far as I undertand, it is correct that the parser removes the leading zero here. However, when parsing a PEM this must be taken into account and the parser must pad the parsed value with a leading zero
            };

            var sut = new Asn1Parser();

            // Act
            var result = asn1Input.SelectMany(sut.Parse).Cast<DerInteger>().Select(i => i.UnencodedValue).ToArray();

            // Assert
            result.Length.Should().Be(expectedParsedValues.Length);
            for (int i = 0; i < expectedParsedValues.Length; i++)
            {
                result[i].Should().Equal(expectedParsedValues[i]);
            }
        }

        [Fact]
        public void Should_serialize_integer_from_bytes()
        {
            // Arrange
            var byteArrays = new byte[][]
            {
                new byte[] {0},
                new byte[] {127},
                new byte[] {0, 127},
                new byte[] {128},
                new byte[] {1, 0, 0},
                new byte[0],
                new byte[] {0, 165, 163, 214, 2, 169, 62}
            }.Select(data => new DerInteger(data));

            var expectedSerializedValues = new[]
            {
                new byte[] {2, 1, 0},
                new byte[] {2, 1, 127},
                new byte[] {2, 2, 0, 127},
                new byte[] {2, 2, 0, 128}, // da das 1. Bit zur Vorzeichenerkennung genutzt wird, wird bei >= 128 ein 0-Byte voran gestellt
                new byte[] {2, 3, 1, 0, 0},
                new byte[] {2, 0},
                new byte[] {2, 7, 0, 165, 163, 214, 2, 169, 62}
            };

            // Act
            var result = byteArrays.Select(i => Asn1.Encode(i)).ToArray();

            // Assert
            result.Length.Should().Be(expectedSerializedValues.Length);
            for (int i = 0; i < expectedSerializedValues.Length; i++)
            {
                result[i].Should().Equal(expectedSerializedValues[i]);
            }
        }
    }
}