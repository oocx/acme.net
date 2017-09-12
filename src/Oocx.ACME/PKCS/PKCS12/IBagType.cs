using Oocx.Asn1PKCS.Asn1BaseTypes;

namespace Oocx.Asn1PKCS.PKCS12
{
    public interface IBagType
    {
        ObjectIdentifier Type { get; }

        IAsn1Element Content { get; }
    }
}