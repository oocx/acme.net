using Oocx.Pkcs;

namespace Oocx.Pkcs.PKCS12
{
    public interface IBagType
    {
        ObjectIdentifier Type { get; }

        IAsn1Element Content { get; }
    }
}