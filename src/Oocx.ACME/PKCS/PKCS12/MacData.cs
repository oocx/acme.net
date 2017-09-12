using Oocx.Pkcs.Asn1BaseTypes;

namespace Oocx.Pkcs.PKCS12
{
    public class MacData : Sequence
    {
        public MacData(DigestInfo mac, OctetString macSalt) : base(mac, macSalt, new Integer(1))
        {

        }
    }
}