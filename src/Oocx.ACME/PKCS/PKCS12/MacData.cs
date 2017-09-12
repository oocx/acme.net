using Oocx.Asn1PKCS.Asn1BaseTypes;

namespace Oocx.Asn1PKCS.PKCS12
{
    public class MacData : Sequence
    {
        public MacData(DigestInfo mac, OctetString macSalt) : base(mac, macSalt, new Integer(1))
        {

        }
    }
}