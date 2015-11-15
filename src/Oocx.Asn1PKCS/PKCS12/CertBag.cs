using Oocx.Asn1PKCS.Asn1BaseTypes;

namespace Oocx.Asn1PKCS.PKCS12
{
    public class CertBag : IBagType
    {
        public CertBag(IAsn1Element content)
        {
            Content = content;
        }

        public ObjectIdentifier Type { get; } = new ObjectIdentifier(Oid.PKCS12.BagType.certBag);
        public IAsn1Element Content { get; }
    }
}