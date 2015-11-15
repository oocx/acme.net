using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Oocx.Asn1PKCS.Asn1BaseTypes
{
    public class Asn1Serializer
    {
        //public IEnumerable<byte> Serialize(IAsn1Entity entity)
        //{
        //    if (entity is IAsn1Element) return Serialize((IAsn1Element) entity);
        //    if (entity is Asn1ComplexType)
        //    {
        //        return ((Asn1ComplexType) entity).Children.SelectMany(Serialize);
        //    }

        //    throw new Exception("Unknown Asn1 entity type: " + entity.GetType().FullName);
        //}

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

            if (element is BitString)
            {
                //yield return ((BitString)element).UnusedBits;
            }

            if (element is Asn1Primitive)
            {                
                foreach (var b in ((Asn1Primitive)element).Data) yield return b;
            }
            else if (element is Asn1Container)
            {
                foreach (var b in ((Asn1Container) element).Children.SelectMany(e => Serialize(e, depth+1)))
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