using System.Collections.Generic;

namespace Oocx.Pkcs
{
    public interface IAsn1Serializer
    {
        IEnumerable<byte> Serialize(IAsn1Element element, int depth = 0);
    }
}