using Oocx.Asn1PKCS.Asn1BaseTypes;

namespace Oocx.Asn1PKCS.PKCS12
{
    public class AuthenticatedSafe : Sequence
    {
        public AuthenticatedSafe(params ContentInfo[] data) : base(data)
        {

        }
    }
}