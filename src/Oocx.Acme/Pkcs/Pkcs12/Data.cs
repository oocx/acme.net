using System;

namespace Oocx.Pkcs
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