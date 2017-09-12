using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Oocx.Pkcs.Asn1BaseTypes
{
    public class Asn1Serializer : IAsn1Serializer
    {        
        public IEnumerable<byte> Serialize(IAsn1Element element, int depth = 0)
        {
            string tabs = "";
            for (int i = 0; i < depth; i++)
            {
                tabs += "\t";
            }
            Trace.WriteLine($"{tabs}{element.GetType().Name} {element.Tag:x} (Total Size: {element.Size} Bytes, Data: {element.Length} Bytes)");
            yield return element.Tag;            

            foreach (var b in element.LengthBytes) yield return b;

            if (element is BitString bitString)
            {
                //yield return ((BitString)element).UnusedBits;
            }

            if (element is Asn1Primitive asn1Primitive)
            {                
                foreach (var b in asn1Primitive.Data) yield return b;
            }
            else if (element is Asn1Container asn1Container)
            {
                foreach (var b in asn1Container.Children.SelectMany(e => Serialize(e, depth+1)))
                {
                    yield return b;
                }
            }
            else
            {
                throw new Exception("Unknown Asn1 element type: " + element.GetType().FullName);
            }
        }                
    }
}