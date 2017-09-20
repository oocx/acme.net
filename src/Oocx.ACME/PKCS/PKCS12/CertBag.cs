using System;

namespace Oocx.Pkcs.PKCS12
{
    public class CertBag : IBagType
    {
        public CertBag(IAsn1Element content)
        {
            Content = content ?? throw new ArgumentNullException(nameof(Content));
        }

        public ObjectIdentifier Type { get; } = new ObjectIdentifier(Oids.BagType.CertBag);

        public IAsn1Element Content { get; }
    }
}