using System;
using System.Security.Cryptography;

namespace Oocx.Pkcs.PKCS12
{
    public class Data
    {
        public Data(OctetString data)
        {
            Content = data ?? throw new ArgumentNullException(nameof(data));
        }

        ObjectIdentifier Type { get; } = new ObjectIdentifier(Oids.Data);

        OctetString Content { get; }
    }
}