using System;

namespace Oocx.Pkcs
{
    public class SafeContentsBag : IBagType
    {
        public SafeContentsBag(IAsn1Element content)
        {
            Content = content ?? throw new ArgumentNullException(nameof(content));
        }

        public ObjectIdentifier Type { get; } = new ObjectIdentifier(Oids.BagType.SafeContentsBag);

        public IAsn1Element Content { get; }
    }
}