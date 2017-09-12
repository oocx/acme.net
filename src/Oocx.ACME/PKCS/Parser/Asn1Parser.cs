using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Oocx.Asn1PKCS.Asn1BaseTypes;

namespace Oocx.Asn1PKCS.Parser
{
    public class Asn1Parser
    {
        public IEnumerable<IAsn1Element> Parse(Stream asn1Stream)
        {
            using (var reader = new BinaryReader(asn1Stream))
            {
                while (reader.PeekChar() > -1)
                {
                    var element = GetAsn1ParsedElement(reader);

                    switch (element.Tag)
                    {
                        case 2:
                            yield return ParseInteger(element);
                            break;
                        case 4:
                            yield return ParseOctetString(element);
                            break;
                        case 48:
                            yield return ParseSequence(element);
                            break;
                        default:
                            yield return new Asn1UnknownElement(element.Tag, element.Data);
                            break;
                    }
                }
            }
        }

        private IAsn1Element ParseOctetString(Asn1ParsedElement element)
        {
            return new OctetString(element.Data);
        }

        private IAsn1Element ParseSequence(Asn1ParsedElement element)
        {
            using (var dataStream = new MemoryStream(element.Data))
            {
                var childElements = Parse(dataStream).ToArray();
                var sequence = new Sequence(childElements);
                return sequence;
            }
        }

        private IAsn1Element ParseInteger(Asn1ParsedElement element)
        {
            if (element.Data.Length == 0)
            {
                return new Integer(new byte[0]);
            }
            if (element.Data[0] == 0)
            {
                if (element.Data.Length == 1)
                {
                    return new Integer(new byte[] { 0 });
                }
                if (element.Data[1] <= 127)
                {
                    return new Integer(element.Data);
                }
                return new Integer(element.Data.Skip(1).ToArray());
            }
            return new Integer(element.Data);
        }

        private static Asn1ParsedElement GetAsn1ParsedElement(BinaryReader reader)
        {
            var tag = reader.ReadByte();
            uint length = (uint)reader.ReadByte();

            if (length > 128)
            {
                length = length - 128;
                switch (length)
                {
                    case 1:
                        length = reader.ReadByte();
                        break;
                    case 2:
                        length = (uint)reader.ReadByte() * 256 + (uint)reader.ReadByte();
                        break;
                    case 3:
                        length = (uint)reader.ReadByte() * 256 * 256 + (uint)reader.ReadByte() * 256 + (uint)reader.ReadByte();
                        break;
                    case 4:
                        length = (uint)reader.ReadByte() * 256 * 256 * 256 + (uint)reader.ReadByte() * 256 * 256 + (uint)reader.ReadByte() * 256 + (uint)reader.ReadByte();
                        break;
                    default:
                        throw new NotImplementedException(
                            "asn elements with more than 4 length bytes are not supported");
                }
            }
            if (length > int.MaxValue)
            {
                throw new NotImplementedException("asn elements with length > int.MaxValue are not supported");
            }
            var data = new byte[length];
            reader.Read(data, 0, (int)length);
            var element = new Asn1ParsedElement { Tag = tag, Data = data };
            return element;
        }

        class Asn1ParsedElement
        {
            public byte Tag { get; set; }

            public byte[] Data { get; set; }

            public override string ToString()
            {
                return $"tag: {Tag} data: {Data.Length} bytes";
            }
        }
    }
}