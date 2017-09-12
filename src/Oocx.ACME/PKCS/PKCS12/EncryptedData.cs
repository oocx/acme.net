using Oocx.Asn1PKCS.Asn1BaseTypes;

namespace Oocx.Asn1PKCS.PKCS12
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