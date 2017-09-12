using System;

using Oocx.Asn1PKCS.Asn1BaseTypes;

namespace Oocx.Asn1PKCS.PKCS12
{
    public class Data
    {
        public Data(OctetString data)
        {
            Content = data ?? throw new ArgumentNullException(nameof(data));
        }

        ObjectIdentifier Type { get; } = new ObjectIdentifier(Oid.PKCS7.data);

        OctetString Content { get; }
    }
}