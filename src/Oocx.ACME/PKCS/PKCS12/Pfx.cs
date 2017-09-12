using Oocx.Asn1PKCS.Asn1BaseTypes;

namespace Oocx.Asn1PKCS.PKCS12
{
    public class Pfx : Sequence
    {
        public Pfx(ContentInfo authSafe, MacData macData) : base(new Integer(3), authSafe, macData)
        {
        }
    }
}