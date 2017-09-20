using System;

namespace Oocx.Pkcs.PKCS12
{
    public class SafeContentsBag : IBagType
    {
        public SafeContentsBag(IAsn1Element content)
        {
            Content = content ?? throw new ArgumentNullException(nameof(content));
        }

        public ObjectIdentifier Type { get; } = new ObjectIdentifier(Oid.PKCS12.BagType.safeContentsBag);

        public IAsn1Element Content { get; }
    }
}