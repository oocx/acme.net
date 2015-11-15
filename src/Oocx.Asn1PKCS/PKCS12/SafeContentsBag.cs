using Oocx.Asn1PKCS.Asn1BaseTypes;

namespace Oocx.Asn1PKCS.PKCS12
{
    public class SafeContentsBag : IBagType
    {
        public SafeContentsBag(IAsn1Element content) 
        {
            Content = content;
        }

        public ObjectIdentifier Type { get; } = new ObjectIdentifier(Oid.PKCS12.BagType.safeContentsBag);
        public IAsn1Element Content { get; }
    }
}