using Oocx.Pkcs.Asn1BaseTypes;

namespace Oocx.Pkcs.PKCS12
{
    public class AuthenticatedSafe : Sequence
    {
        public AuthenticatedSafe(params ContentInfo[] data) : base(data)
        {

        }
    }
}