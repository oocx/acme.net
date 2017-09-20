using System;

namespace Oocx.Pkcs
{
    public class EncryptedData
    {
        public EncryptedData(OctetString data)
        {
            Content = data ?? throw new ArgumentNullException(nameof(data));
        }

        ObjectIdentifier Type { get; } = new ObjectIdentifier(Oids.EncryptedData);

        OctetString Content { get; }
    }
}