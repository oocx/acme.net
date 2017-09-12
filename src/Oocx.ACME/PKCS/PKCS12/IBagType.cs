using Oocx.Pkcs.Asn1BaseTypes;

namespace Oocx.Pkcs.PKCS12
{
    public interface IBagType
    {
        ObjectIdentifier Type { get; }

        IAsn1Element Content { get; }
    }
}