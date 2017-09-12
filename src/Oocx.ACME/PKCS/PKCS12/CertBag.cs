using System;
using Oocx.Pkcs.Asn1BaseTypes;

namespace Oocx.Pkcs.PKCS12
{
    public class CertBag : IBagType
    {
        public CertBag(IAsn1Element content)
        {
            Content = content ?? throw new ArgumentNullException(nameof(Content));
        }

        public ObjectIdentifier Type { get; } = new ObjectIdentifier(Oid.PKCS12.BagType.certBag);

        public IAsn1Element Content { get; }
    }
}