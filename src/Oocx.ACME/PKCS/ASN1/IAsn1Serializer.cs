using System.Collections.Generic;

namespace Oocx.Pkcs.Asn1BaseTypes
{
    public interface IAsn1Serializer
    {
        IEnumerable<byte> Serialize(IAsn1Element element, int depth = 0);
    }
}