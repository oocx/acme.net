using Oocx.Pkcs.Asn1BaseTypes;

namespace Oocx.Pkcs.PKCS12
{
    public class EncryptedData
    {
        public EncryptedData(OctetString data)
        {
            Content = data;
        }

        ObjectIdentifier Type { get; } = new ObjectIdentifier(Oid.PKCS7.encryptedData);

        OctetString Content { get; }
    }
}