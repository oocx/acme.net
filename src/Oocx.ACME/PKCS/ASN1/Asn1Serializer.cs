using System;
using System.IO;

namespace Oocx.Pkcs
{
    public class Asn1Serializer
    {
        public byte[] Serialize(IAsn1Element element, int depth = 0)
        {
            using (var ms = new MemoryStream())
            {
                Serialize(element, ms);

                return ms.ToArray();
            }
        }

        public void Serialize(IAsn1Element element, Stream stream, int depth = 0)
        {
            /*
            string tabs = "";
            for (int i = 0; i < depth; i++)
            {
                tabs += "\t";
            }

            Trace.WriteLine($"{tabs}{element.GetType().Name} {element.Tag:x} (Total Size: {element.Size} Bytes, Data: {element.Length} Bytes)");
            */

            // Tag
            stream.WriteByte(element.Tag);

            // Length bytes
            stream.Write(element.LengthBytes, 0, element.LengthBytes.Length);

            if (element is BitString bitString)
            {
                // yield return ((BitString)element).UnusedBits;
            }
            if (element is Asn1Object asn1Primitive)
            {
                stream.Write(asn1Primitive.Data, 0, asn1Primitive.Data.Length);
            }
            else if (element is Asn1Container asn1Container)
            {
                foreach (var b in asn1Container.Children)
                {
                    Serialize(b, stream, depth + 1);
                }
            }
            else
            {
                throw new Exception("Unknown Asn1 element type: " + element.GetType().FullName);
            }
        }                
    }
}